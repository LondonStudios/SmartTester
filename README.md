![London Studios](https://i.ibb.co/1mwSS1q/Untitled-design.png)

# London Studios - Update
Since forming London Studios in April 2020 we've created a number of **high quality** and **premium** resources for the FiveM project, focusing on the emergency services and aiming to bring your server to the next level.

Although we made a number of free resources such as this one in the first year, we've now switched to creating paid content, keeping them constantly updated and working along with providing the best possible support to our customers.

Our **most popular** resources now include *Smart Fires, Police Grappler* and *Smart Hose*.

With **thousands** of **happy customers** we are confident you'll love our resources and our active support team are on hand to help if you have any questions!

# Our store: https://store.londonstudios.net/github
# Our discord: https://discord.gg/nC2krpN

Therefore, this resource is now likely *out of date* and is *no longer supported by us*. The full source code is available should you wish to make any changes. All of our paid resources however are constantly updated and we invite you to take a look!

# SmartTester - London Studios
**SmartObservations** is a **FiveM** resource coded in **C#** enhancing the roadside testing experience. The resource includes a customisable **Breathalyser** and **Drugalyser** including realistic sounds, the ability to fail to provide a specimen and a delay before you receive the drug result. All of which can be customised in the config.ini The plugin also extends your arm in-game when you are taking a sample.

View a demonstration video [here](https://www.youtube.com/watch?v=V2v2rFwmvRg&feature=youtu.be).

If you enjoy my work, feel free to **buy me a coffee**.

<a href="https://www.buymeacoffee.com/londonstudios" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/arial-orange.png" alt="Buy Me A Coffee" style="height: 31px !important;width: 197px !important;" ></a>

If you enjoy my plugins, buy me a coffee!
Join our **discord** [here](https://discord.gg/AtPt9ND).

This plugin is made by LondonStudios, we have created a variety of releases including TaserFramework, SearchHandler, ActivateAlarm and more!


![SmartTester](https://i.imgur.com/EjhZL4h.png)

## Usage
**/breathalyse** - Takes a sample of breath from the nearest player.
**/drugalyse** - Takes a saliva sample from the nearest player.
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

## Feedback
We appreciate feedback, bugs and suggestions related to SmartTester and future plugins. We hope you enjoy using the resource and look forward to hearing from people!
