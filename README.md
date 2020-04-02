# :bus: TheBus

A generic interface to talk to a service bus. (Queues, Topics, and Subscriptions) *Taken from the public transportation all over Hawai'i.*  [TheBus](http://www.thebus.org/), my primary mode of transportation growing up.

## Goal

Create a **simple** interface to talk to a service bus.

### Constraints

* Use [Visual Studio Code](https://code.visualstudio.com/), mostly to get me out of my comfort zone (Visual Studio) and since this is a free library I want to make sure that it can be referenced through a free Integrated Development Environment (IDE).

* Share as much code as I can without it being **totally** unmaintainable. In a previous instance of this code I used a **ton** of `#ifdef`'s and it was just getting too unwieldly. I **really** had to know what file/project I was clicking on to see the active build code (.Net Core or Framework).  The source file was in the .Net Framework project and the .Net Core code contained links to the files in the Framework project.

* [.NET Core](https://dotnet.microsoft.com/download/dotnet-core) is primary.

* .NET Framework is secondary, but supported.

* Proper use of [Depenency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) (DI) and Inversion of Control (IoC).

## Why wrap code?

* Simple interface (encapsulation - hides complexity)
* Embed knowledge (defaults)
* Common interface across multiple implemenations (Core, Framework / Azure, AWS)
* Decouple business logic from a framework
* Less client code

## TODO

* Fill in the implementation classes and possibly tweak the interface based on usage.
* Add mocks for testing.
* Add [Testing Extension](https://code.visualstudio.com/api/working-with-extensions/testing-extension)
* Download and test on my Mac Book Pro. (*it should just run*)

## Visual Studio Code Extensions

The following are a list of VS Code Extensions that I use for this project as well as other projects.

* **markdownlint** - Helps with standardizing your Markdown, but some of the standards are a little off in my opinion.

* **C#** - A **MUST** have when writing .Net code.

## :book: Reference :books:

* [.Net Core](https://docs.microsoft.com/en-us/dotnet/core/)
* [Markdown Guide](https://www.markdownguide.org/basic-syntax) - Because I **forget**...  a **LOT** :laughing:
* [Emoji Cheat Sheet](https://www.webfx.com/tools/emoji-cheat-sheet/) :smile: