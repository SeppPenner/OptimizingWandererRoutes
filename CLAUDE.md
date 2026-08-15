# Project rules for Claude

## What this is

OptimizingWandererRoutes is a small console program that solves the "wanderer" homework task
[Cipsoft](https://www.cipsoft.com) handed out as an application exercise. The task definition is
`Hausaufgabe-Programmierer.pdf` in the repository root: a hiker walks a fixed route with fixed
stages between the possible places to stay overnight, the route has to be split into a given number
of days, and the longest day has to become as short as possible. Stages are consecutive, only the
cut points between days are free.

The program reads an input file whose first line is the number of stages, whose second line is the
number of days and whose remaining lines are the stage lengths in kilometers. It prints the sum per
day and the maximum of those sums. `src/OptimizingWandererRoutes/in2.txt` is exactly the example
from the PDF and must keep printing `11`, `26`, `22` and `Maximum: 26`.

The repository is an example program, it is **not** published as a NuGet package and it has **no**
installer: no `GeneratePackageOnBuild`, no push script, no Inno Setup, no publish batch file.

One solution `src/OptimizingWandererRoutes.sln` with exactly two projects:

- `src/OptimizingWandererRoutes/OptimizingWandererRoutes.csproj`, `OutputType` `Exe`, the whole
  program.
- `src/OptimizingWandererRoutes.Tests/OptimizingWandererRoutes.Tests.csproj`, MSTest, added in
  version 1.0.8.0.

Layout inside `src/OptimizingWandererRoutes`:

- `Program.cs`: `Main` asks for a file name on the console, drives `ReadFile`, `Optimize` and
  `PrintResults` and catches the four exception types plus `Exception`.
- `Optimizer.cs` plus `IOptimizer.cs`: the algorithm. `ReadFile` parses the input file into
  `numberOfStages`, `numberOfDays` and `stages`, `FillBuckets` builds the initial split,
  `OptimizeRoutes` balances neighbouring days and `PrintResults` writes the result to the console.
- `Bucket.cs`: one day of the route, a `List<int>` of stages with access to its leftmost and
  rightmost element. It has no interface, unlike `Optimizer`.
- `Exceptions/FileNotReadException.cs`, `Exceptions/OptimizeNotCalledException.cs`,
  `Exceptions/TooLessStagesException.cs`: three exceptions with the usual three constructors each.
- `GlobalUsings.cs`: all usings of the project.
- `in.txt`, `in2.txt`, `in3.txt`, `in4.txt`: sample inputs, see the quirk about the working
  directory below.

Layout inside `src/OptimizingWandererRoutes.Tests`:

- `OptimizerTests.cs`: the example of the PDF, the two uneven splits that used to produce an extra
  day, the single day, the ignored surplus stages and the four error cases.
- `BucketTests.cs`: the empty bucket, the order of the stages, removing from both ends and the rule
  that the last stage of a day stays.
- `TestDataProvider.cs`: the stages of the PDF example and the writer for the input files. Both test
  classes use it.
- `GlobalUsings.cs`: all usings of the test project.

The tests need no fixture file. `OptimizerTests` writes its input files into its own directory below
`Path.GetTempPath()` and deletes it afterwards, so a test run leaves the working tree untouched.

Repository root: `Readme.md` (the only user documentation), `Changelog.md`, `License.txt` (MIT),
`Hausaufgabe-Programmierer.pdf` (the task), `HowTheAlgorithmWorks.xlsx` and `Explanation1.png` to
`Explanation4.png` (the algorithm walked through step by step, linked from the Readme),
`.gitattributes` and `.gitignore`. There is no `Updating.md`, no `HowToUse.md` and no `.github`
folder.

## Build

```powershell
dotnet build src/OptimizingWandererRoutes.sln -c Release
```

```powershell
dotnet test src/OptimizingWandererRoutes.sln
```

- Single target framework `net10.0` in both projects, no multi-targeting, no `RuntimeIdentifiers`.
  Nothing in the code is Windows specific.
- All build properties live directly in the two `.csproj` files and are duplicated there. There is
  **no** `Directory.Build.props` in this repository.
- `TreatWarningsAsErrors` is enabled, so every warning breaks the build, NuGet warnings (`NU****`)
  from restore included. A clean build reports zero warnings, keep it that way.
- `NU1803` (HTTP source usage during restore) is the one warning suppressed via `NoWarn`. Fix
  warnings instead of extending that list. `NuGetAudit` and `NuGetAuditMode=all` are on, so a
  vulnerable transitive package fails the build too.
- Versions come from GitVersion.MsBuild out of the git tags, for example `1.0.9-1` for the first
  commit after tag `1.0.8`. Never edit a version property or an assembly version by hand.
- Restore needs nuget.org. If a private feed is configured globally on the machine and answers 404
  for public packages, restore fails with `NU1301`. Then build with an explicit source:
  `dotnet build src/OptimizingWandererRoutes.sln --source https://api.nuget.org/v3/index.json`.
- Tests are MSTest, in the single test project `src/OptimizingWandererRoutes.Tests`, which follows
  the same package set as the sibling repositories: `Microsoft.NET.Test.Sdk`, `MSTest.TestAdapter`,
  `MSTest.TestFramework`, `coverlet.collector` and `GitVersion.MsBuild`. `dotnet test` runs 14
  tests, they need no network.
- Beyond the tests, a behaviour change is verified by running the program against the four
  `in*.txt` files, `in2.txt` first, because that one has a known correct result in the PDF. Never
  claim a run happened without running it.

## Code conventions

Follow the surrounding code, it is consistent throughout every file:

- File header comment block with `<copyright file="..." company="Hämmer Electronics">` and a
  `<summary>`, then the file-scoped namespace.
- XML doc comments on every type and every member, private fields and private methods included, no
  exceptions. Implementations of an interface member additionally carry `<inheritdoc cref="..."/>`
  and `<seealso cref="..."/>` pointing at that interface. The exception classes use
  `<inheritdoc cref="Exception"/>` on their constructors.
- `Nullable`, `ImplicitUsings` and `LangVersion latest` are enabled.
- New `using` directives go into `GlobalUsings.cs`, inside the existing `#pragma warning disable
  IDE0065` block, never at the top of a file. The editorconfig requires usings inside the namespace
  (`csharp_using_directive_placement=inside_namespace:warning`), which global usings cannot satisfy,
  that is what the pragma is for. Do not add other pragmas. The comment text in that block is German
  because Visual Studio generated it, leave it alone.
- Fields, properties, methods and events are always accessed with `this.` qualification
  (`dotnet_style_qualification_for_*` at severity `warning`).
- The method bodies of `Optimizer` are commented line by line in plain prose. That density is
  intentional here, it is what makes the algorithm followable next to the Excel file. Keep it when
  touching those methods.
- `src/.editorconfig` also enforces braces everywhere, no multiple blank lines, four spaces, CRLF,
  UTF-8, file scoped namespaces, `System` usings sorted first and `IDE0005` as warning. Analyzer
  warnings are fixed, not silenced.

## Known quirks

Do not silently "clean up" these, they are existing behaviour:

- **`OutputType` has to stay `Exe`.** Up to version 1.0.7.0 it was `WinExe`, which gave the built
  exe the Windows GUI subsystem (PE subsystem `2`). A double click in the Explorer then opened no
  console at all: `Console.WriteLine` went nowhere, `Console.ReadLine` returned `null` immediately
  and the program ended without a visible trace. Only a start from an already existing console
  worked, because the process inherits that console. Do not set it back.
- **The input files are not copied to the output directory.** `in.txt` to `in4.txt` have no
  `CopyToOutputDirectory`, so `bin/Release/net10.0` contains only the assemblies. The program
  resolves the typed file name against the current working directory, which means it has to be
  started from `src/OptimizingWandererRoutes` for a bare `in2.txt` to be found, or the full path
  has to be typed.
- **The last day swallows the rest.** `FillBuckets` computes the elements per bucket once and then
  opens a new bucket whenever that count is reached, but only while fewer buckets than
  `numberOfDays` exist. As soon as the last day is open, every remaining stage is appended to it.
  That guard used to apply to the case of exactly one element per bucket only, which is why version
  1.0.7.0 and earlier printed a `3.Tag` for 5 stages over 2 days and a `4.Tag` for 7 stages over 3
  days. Keep the guard in front of all other cases.
- **The optimization is a heuristic, not an exact solution.** `OptimizeRoutes` only ever moves the
  outermost stage between two neighbouring days and stops as soon as no single move improves
  anything, so it finds a local optimum. For the stages 1 to 7 over 3 days it reports a maximum of
  13 km while 11 km is possible. The PDF example in `in2.txt` happens to come out optimal. Making
  this exact would mean replacing the algorithm the Excel file and the four screenshots explain,
  that is a rewrite of the repository, not a bug fix.
- **The `.5` string check.** `FillBuckets` decides how to round by formatting the quotient and
  asking whether the string ends in `.5`, with `CultureInfo.InvariantCulture` so that a German
  locale does not turn the point into a comma. It is a strange way to write "round half down", it is
  the documented behaviour of the algorithm in `HowTheAlgorithmWorks.xlsx`.
- **`-1` as a sentinel.** `Bucket` returns `-1` from `GetLeftMostElement` and `GetRightMostElement`
  when it is empty, and from `RemoveLeftMostElement` and `RemoveRightMostElement` when one single
  element is left. The second case is not an error, it is the rule that a day may never lose its
  last stage. A negative stage length cannot occur, so the sentinel is unambiguous.
- **The output has no space after the day number.** `PrintResults` writes `1.Tag: 11 km`, the PDF
  shows `1. Tag: 11 km`. The numbers match, the spacing does not.
- **Extra lines in the input file are ignored.** `ReadFile` reads every line and truncates the list
  to `numberOfStages` afterwards, so `in.txt` with its seven stage lines for six stages drops the
  last one. Fewer lines than announced is the error case and throws `TooLessStagesException`.
- **The four catch blocks in `Program.cs` all do the same thing.** They print the message and wait,
  exactly like the final `catch (Exception)`. They only exist to name the expected error cases.
- **`PrintResults` waits for input.** It ends with `Console.ReadLine`, so anything calling it
  blocks until a line arrives. That matters for automated runs, pipe an empty line in. It is also
  the reason why `OptimizerTests` replaces `Console.In` along with `Console.Out`: the result can
  only be checked by reading the console output back, and the run must not hang while doing it.
- **AppVeyor badge without CI in the repository.** `Readme.md` links an AppVeyor build that is
  configured outside of this repository. There is no pipeline file here.
- **`src/OptimizingWandererRoutes.sln.DotSettings`** is tracked and holds nothing but a ReSharper
  user dictionary with the single entry `H_00E4mmer`. Leave it alone.

## Releasing

1. Make the change.
2. Add an entry at the top of `Changelog.md` in the existing format:
   `* **Version 1.0.8.0 (2026-08-15)** : Short description.`
3. Commit that.
4. Tag the commit with the plain version number, no `v` prefix (`1.0.7`, `1.0.6`, ...). The existing
   tags are lightweight tags, create new ones the same way.
5. Push the commits and the tag.

The version in the `Changelog.md` has four parts (`1.0.8.0`), the tag has three (`1.0.8`).
GitVersion turns the tag into the assembly version, so an untagged commit produces something like
`1.0.8-1+Branch.master.Sha...`. There is no installer to build and no package to push, so the
release ends with the push.

## Git

- **Never amend a commit.** No `git commit --amend`, not for a typo in the message, not to add a
  forgotten file, not even when the commit is still local. Write a follow-up commit instead. The
  release versions come from tags on exact commits, an amended commit leaves its tag pointing at a
  commit that no longer exists in the branch.

## Writing style

- Commit messages are written **in English only**: short, precise subject line, explanatory body
  when needed.
- Code comments and comments in project files such as `.csproj` are **always English**, regardless
  of the language used in the conversation.
- **No em dashes or en dashes** (`—`, `–`), neither in prose, commit messages, code comments nor
  documentation. Use a regular hyphen, comma, colon, parentheses or a separate sentence.
- German texts (documentation, chat replies) always use real umlauts and ß, never ASCII
  transliterations such as `ae`, `oe`, `ue` or `ss`. Identifiers, file names and configuration keys
  stay unchanged where umlauts are technically undesirable.
