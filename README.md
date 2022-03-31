# Welcome to Robobobot

Robobot is a multiplayer battle game where you control your tank via API calls. You can do it manually, but you should really create a bot.

> It's under development, and it's really not far along. The plan is to start with the sandboxed mode to get the API defined.

The server is written in .net 6. There is a dockerfile present if you want to run it in docker.

## Contribute?

Check out the [contribute](docs/Contribute.md) page...

## What's done vs planned?

The rough plan.

- [x] API for creating a sandbox battle
- [ ] Iterate over the API design
- [ ] Some kind of security to avoid bots from reading the visualization APIs
- [ ] Create a visualization
  - [x] Simple text visualization
  - [ ] Fancier web based to view the battle on a big screen
- [ ] Create a Deep State (organizer) website
- [ ] Add client packages to make it easier to get started
  - [ ] C#
  - [ ] TS/JS
  - [ ] ?

## How do I play?

### As deep state (aka the organizer)

1. Create a new battle
2. Create players (each get a generated token to use in the API calls as credentials).
3. Hand out Battle Id and Player Tokens and lean back.

### As a player

1. You have a tank.
2. You control said tank via an REST API (like in real life). You can choose any framework or runtime you would like.
3. Each call costs different amount of time.
4. You can only have one pending request at the time.

### Training you bot

The rest API has a sandbox mode where you can set the stage for your tank and run it as many times as you'd like. See the [training docs](docs/Training.md).

## Getting started

1. Clone the repo.
2. Compile and run `Robobobot.Server`
