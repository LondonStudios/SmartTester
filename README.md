# SmartTester - London Studios
**SmartObservations** is a **FiveM** resource coded in **C#** enhancing the roadside testing experience. The resource includes a customisable **Breathalyser** and **Drugalyser** including realistic sounds, the ability to fail to provide a specimen and a delay before you receive the drug result. All of which can be customised in the config.ini The plugin also extends your arm in-game when you are taking a sample.

View a demonstration video [here](https://www.youtube.com/watch?v=V2v2rFwmvRg&feature=youtu.be).

If you enjoy my work, feel free to **buy me a coffee**.
<a href="https://www.buymeacoffee.com/londonstudios" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/arial-orange.png" alt="Buy Me A Coffee" style="height: 31px !important;width: 197px !important;" ></a>

This plugin is made by LondonStudios, we have created a variety of releases including TaserFramework, SearchHandler, ActivateAlarm and more!

![SmartTester](https://i.imgur.com/EjhZL4h.png)

## Usage
**/breathalyse [PlayerId]** - Takes a sample of breath from another player.
**/drugalyse [PlayerId]** - Takes a saliva sample from another player.
**/setbreath [result]** - If you are being breathalysed, you'll be prompted in chat to use this command.
**/setdrug [Cannabis (true/false) Cocaine (true/false)]** - If you are being drugalysed, you'll be prompted in chat to use this command. eg, /setdrug true true
**/failprovide** - If you are being sampled, this will fail to provide and the officer will be notified.

After drugalysing someone, the default value in the config is set to 90 seconds, meaning the plugin will wait this duration before releasing the result to you.

## Installation

 1.  Create a new **resource folder** on your server.
 2.  Add the contents of **"resource"** inside it. This includes:
"Client.net.dll", "Server.net.dll", "fxmanifest.lua", "SharpConfig.dll", "html".
3. In **server.cfg**, "ensure" SmartTester, to make it load with your server startup.
## Configuration
The plugin is customisable in terms of the units and the drugs tested by the drugalyser. Please open the config.ini file to make edits.

    [SmartTesting]
    # Breathalyser
    breathalyserUnit = µg/100ml
    breathalyserLimit = 35
    
    # Drugalyser
    drugalyserDrug1 = Cannabis
    drugalyserDrug2 = Cocaine
    drugalyserDelay = 90 #seconds

For example, if you want to remove the delay, change 90 to 0. You can change the breathalyser Unit or limit above, they are currently set to the UK limits and unit. You can also change the drug names it detects for.

## Source Code
Please find the source code in the **"src"** folder. Please ensure you follow the licence in **"LICENCE.md"**.

## Discord
<iframe src="https://discordapp.com/widget?id=710224003054436394&theme=dark" width="350" height="500" allowtransparency="true" frameborder="0"></iframe>

## Feedback
We appreciate feedback, bugs and suggestions related to SmartTester and future plugins. We hope you enjoy using the resource and look forward to hearing from people!