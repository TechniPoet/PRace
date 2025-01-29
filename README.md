Welcome to PeloRace

This game was developed on PC and tested on MacInCloud https://www.macincloud.com

This project *should* work with both keyboard/mouse and gamepad, however most testing was done with keyboard.

##General Overview##
This project was build upon the principle of data driven development to allow for easy modification for multiple controller types and/or hooking a server up to run all the game simulation logic.
Speed will always increase or decrease depending on the last input recieved.

##General Overview##
Architecturally I aimed to provide 1 directional communication. API's feed data to services which also act as ViewModels (as complexity grows, this functionality could be split up in the future), and Services feed data to Views. 
In this setup services act as the middle man between input and the API, so while services can communicate with they API, they only pass along commands with no assumption on the ramifications.

Configs reside as Scriptable objects but this data could be split between server configs for simulation data and scriptable objects for client side presentation data.
- GameConfig
- PlayerConfig
- RaceConfig

Api
- GameRunner

Services
- Input Service
- RowerViewService
- UIService

Views
- GameOverView
- GameUIView
- RowerView
