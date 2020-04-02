# TheBus
A generic interface to talk to a service bus. (Queues, Topics, and Subscriptions) Taken from the public transportation all over Hawai'i.  [TheBus](http://www.thebus.org/), my primary mode of transportation growing up.

## Goal

Create a **simple** interface to talk to a service bus.

### Constraints

* Use Visual Studio Code, mostly to get me out of my comfort zone (Visual Studio) and since this is a free library I want to make sure that it can be referenced through a free Integrated Development Environment (IDE).
* Share as much code as I can without it being **totally** unmaintainable.
* .Net Core is primary.
* .Net Framework is secondary, but supported.


## Why wrap code?

* Simple interface (hides complexity)
* Embed knowledge (defaults)
* Common interface across multiple implemenations (Core, Framework)
* Decouple business logic from a framework
* Less client code