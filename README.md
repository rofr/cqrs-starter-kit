# Domain Driven Design example using OrigoDB (was Edument CQRS and Intentful Testing Starter Kit)

## What is this?

This is a heavily modified fork of the edumentab/cqrs-starter-kit sample application demonstrating an alternative architecture based on  OrigoDB. It seemed like a good idea to compare the same application written with two very different styles.

Modifications:
* Deleted everything except the sample application
* CQRS, Aggregates, Domain Events and Event Sourcing gone
* Single simple model for both reads and writes, all ACID
* Redesigned core using basic OO modeling and transaction script pattern
* Rewrote tests using Arrange/Act/Assert pattern
* Glued together with the UI, see Domain.cs in the WebFront project


## Who did this?

The original code was built by Jonathan Worthington and Carl Mäsak as part of their
work at [Edument](http://www.edument.se/).
