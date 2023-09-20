# Tickets

## Goal

This repo is a result of some sessions that we had regarding "DDD oriented microservices" where we took same fictional company and start to break down into boundaries and microservices. The goal was to study together the possibilities and impacts of how we split company's domain into microservices and we took one microservice to go deeper and create its domain model.

## Scope of our code

We had scoped our code more into: Aggregates, Entities, Domain Events, Event handling.
So, keep in mind that all the rest is far from our daily standard.

## Running

You can actually run this application and reserve session seat, create a movie session and create a TicketOrder (with the desired session seats). Application is relying on a SQLite file that generates on the first run and you can delete any time you want to restart the state. You can easily access a SQLite instance using Datagrip.

## Example calling endpoints

Inside folder "thunderclient", you can find file that you can import into VS Code's [Thunder Client](https://marketplace.visualstudio.com/items?itemName=rangav.vscode-thunder-client) extension.
