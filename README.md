# :bus: TheBus

A generic interface to talk to a service bus. (Queues, Topics, and Subscriptions) *Taken from the public transportation all over Hawai'i.*  [TheBus](http://www.thebus.org/), my primary mode of transportation growing up.

## Goal

Create a **simple** interface to talk to a service bus.

### Constraints

* Use [Visual Studio Code](https://code.visualstudio.com/), mostly to get me out of my comfort zone (Visual Studio) and since this is a free library I want to make sure that it can be referenced through a **free** Integrated Development Environment (IDE).

* Share as much code as I can without it being **totally** unmaintainable. In a previous instance of this code I used a **ton** of `#ifdef`'s and it was just getting too unwieldly. I **really** had to know what file/project I was clicking on to see the active build code (.Net Core or Framework).  The source file was in the .Net Framework project and the .Net Core code contained links to the files in the Framework project.

* [.NET Core](https://dotnet.microsoft.com/download/dotnet-core) is primary.

* .NET Framework is secondary, but supported.

* Proper use of [Depenency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) (DI) and Inversion of Control (IoC).

## Why wrap code?

* Simple interface (encapsulation - hides complexity)
* Embed knowledge (defaults)
* Common interface across multiple implemenations (Core, Framework / Azure, AWS)
* Decouple business logic from a framework so changing implementations isn't as difficult
* Less client code

## `dotnet` Command Line Interface

* `dotnet new` - Helps you create console apps, class libraries, etc.  This will scaffold a `.csproj` file.
* `dotnet add` - Adds project or package reference(s) to an existing `.csproj` file.
* `dotnet build`

## Adding a GitHub NuGet Package Reference

So you've built a package in GitHub and **now** you want to reference it.  You should be able use `dotnet add`. [Reference](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package)

        dotnet add package

You may not be able to `add` the package due to the GitHub credentials, but once you add those credentials the `restore` will work.
Jamie @ GitHub support says that PAT's will be stripped from any `nuget.config` file, but you can XML encode them so they stick.
I would suggest putting any credentials in your global `nuget.config` file typically found in the your Nuget's home directory
*(~/AppData/Roaming/NuGet/NuGet.config)*.

        dotnet tool update gpr -g
        gpr xmlEncode <YOUR_READ_PACKAGES_TOKEN>

## TODO

* Fill in the implementation classes and possibly tweak the interface based on usage
* Add mocks for testing
* Add [Testing Extension](https://code.visualstudio.com/api/working-with-extensions/testing-extension)
* Download and test on my Mac Book Pro. (*it should just run*)

## Visual Studio Code Extensions

The following are a list of VS Code Extensions that I use for this project as well as other projects.

* **C#** - A **MUST** have when writing .Net code.

* **markdownlint** - Helps with standardizing your Markdown, but some of the standards are a little off in my opinion.

## :book: Reference :books:

* [.Net Core](https://docs.microsoft.com/en-us/dotnet/core/)
* [Markdown Guide](https://www.markdownguide.org/basic-syntax) - Because I **forget**...  a **LOT** :laughing:
* [Emoji Cheat Sheet](https://www.webfx.com/tools/emoji-cheat-sheet/) :smile:
