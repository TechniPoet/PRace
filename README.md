Welcome to PeloRace

This game was developed on PC and tested on MacInCloud https://www.macincloud.com so if anything does not run appropriately, please reach out.

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

##3 Unity Features##
Key unity features used
###UI###
One of the largest set backs I see unity devs run into early on is not setting up ui to be dynamic to resolution or aspect ratio. My key use of anchoring setup and hierarchy organization can save lots of time later on in the developemnt process.

###Testing###
Unity Provides testing tools that many people tend to ignore. Particularly for data driven developent; Taking advantage of this testing suite is key to ensuring safe builds protected from unintended consequences. I have provided unit tests for all simulation code.

###Unity Input###
Even in the past year I have run into lots of devs who haven't jumped into the new input system. While not used extensively, understanding its modularity provides a useful tool to utilize multi-controller input that is accessible to designers while giving ease of use to programmers who don't want to have to write code everytime the controls change or a new input type is provided.

###Scriptable Objects###
Often overlooked, scriptable objects are a key tool to provide data driven development in a way that allows object references, easier a/b testing, and safer git merge conflict handling (so you don't have to worry about constant scene changes.) 
While not displayed in this project due to its small scope and low complexity, it can be leveraged in combination with the custom import system to even drive single source cross server/client data that also holds context specific unity references for easier iteration for backend/front end developers.

##Final Thoughts##
I feel as if the code provided is tested/commented enough to speak to it's own functionality. Please don't hesitate to contact me for any clarification or troubleshooting required.

Contact: RobbinsJeffreyM@gmail.com
