# Advnaced Distributed System Design (notes)

## Fallacies of Distributed Computing

* Fallacy #1: The network is reliable
* Fallacy #2: Latency isn't a problem
* Fallacy #3: Bandwidth isn't a problem
* Fallacy #4: The network is secore
* Fallacy #5: The network topology won't change
* Fallacy #6: The admin will know what to do
* Fallacy #7: Transport cost isn't a problem
* Fallacy #8: The network is homogenious
* Fallacy #9: The system is atomic
* Fallacy #10: The system is finished
* Fallacy #11: Towards a better development
* Fallacy #12: The business logic can and should be centralized

## Coupling

Either I like it or I don't like it.  A measurment of dependencies.  Differenct perspectives.

### Coupling in applications: Afferenct & Efferent

* Afferent coupling (Ca) - who depends on you
  * Incoming coupling
* Efferent coupling (Ce) - who do you depend on
  * Outgoing coupling

### Coupling in systems: platform, temporal, and spatial

### Coupling solutions: platform

### Coupling solutions: temporal and spatial

## Introduction to Messaging

### Why messaging?

### One-way, fire & forget

### Performance: messaging vs RPC

### Service interfaces vs strongly-typed

### Fault tolerance

### Auditing

### Web Services invocation

## Exercise: selling messaging to your organization

## Messaging Patterns

### Dealing with out of order messages

* An application's concern, because the domain rules will limit this scope.
* Many messaging frameworks will provide sequencing which is a complex problem to solve.

### Request-response

* **Return Address** - useful for returnin the response because the connection has been lost after the request.

### Publish-subscribe (pg 88)

* Name it Sub / Pub?  Need to subscribe first and **then** publish to the message.
* De-coupling, temporal, delivery to scaled out machines (back end vs front end caching)

### Publish-subscribe: topics

#### Out of order messages (pg 95)

### Visualization

## Architecural styles: Bus and Broker

### Broker

* Hub & Spoke (vs point to point integration)
* Broker is physically separate
* Single point of failure
* SOA was the response to the failings of Broker (BizTalk, etc.)

### Bus

* Event sources / Event sinks (producers & consumers)

## Intro to SOA

## Exercise: services modelling

## Advanced SOA

## CQRS

## SOA: operational aspects

## Sagas/Long-running business processes modelling

## Exercise: saga design

## SOA: modelling

## Organizational transition to SOA

## Web Services and User Interfaces
