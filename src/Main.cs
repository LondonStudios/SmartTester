﻿using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using SharpConfig;

using static CitizenFX.Core.Native.API;

namespace SmartTesting
{
    public class Main : BaseScript
    {
        public string bUnit;
        public float bLimit;
        public string dUnit;
        public string d1;
        public string d2;
        public int dDelay;

        public bool awaitingBreath;
        public int requestId;
        public float breath;

        public int[] breathColour = { 235, 171, 52 };
        public int[] drugColour = { 52, 168, 235 };

        public bool awaitingDrug;
        public int drugRequestId;
        public bool sent = false;

        public int playerLoop = 64;

        public Dictionary<int, int> siren_luxstate = new Dictionary<int, int> { };
        public Main()
        {
            if (GetConvar("onesync_enabled", "true") == "true")
            {
                playerLoop = 256;
            }

            ReadConfig();

            EventHandlers["Client:RequestDrug"] += new Action<int, int, Vector3>((local, request, location) =>
            {
                if (local == GetPlayerServerId(PlayerId()))
                {
                    var localLocation = GetEntityCoords(PlayerPedId(), true);
                    if (localLocation.DistanceToSquared(location) > 7f)
                    {
                        TriggerServerEvent("Server:ReturnDrug", local, request, false, false, false, false);
                    }
                    else
                    {
                        if (awaitingDrug)
                        {
                            TriggerServerEvent("Server:ReturnDrug", local, request, false, false, false, false);
                        }
                        else
                        {
                            awaitingDrug = true;
                            drugRequestId = request;
                            TriggerEvent("chat:addMessage", new
                            {
                                color = drugColour,
                                args = new[] { "[Drugalyser]", "You are being drugalysed. Use /setdrug or /failprovide." }
                            });
                        }
                    }
                }
            });

            EventHandlers["Client:ReturnDrug"] += new Action<int, int, bool, bool, bool, bool>((local, request, cannabis, cocaine, failprovide, success) =>
            {
                if (request == GetPlayerServerId(PlayerId()))
                {
                    if (success == false)
                    {
                        PlaySoundFrontend(-1, "Place_Prop_Fail", "DLC_Dmod_Prop_Editor_Sounds", false);
                        TriggerEvent("chat:addMessage", new
                        {
                            color = drugColour,
                            args = new[] { "[Drugalyser]", "You are unable to drugalyse this person." }
                        });
                    }
                    else
                    {
                        if (failprovide)
                        {
                            TriggerEvent("chat:addMessage", new
                            {
                                color = drugColour,
                                args = new[] { "[Drugalyser]", "The suspect has failed to provide a specimen of saliva." }
                            });
                        }
                        else
                        {
                            string cannabisResult;
                            string cocaineResult;
                            if (cannabis)
                            {
                                cannabisResult = "^8FAIL^7";
                            }
                            else
                            {
                                cannabisResult = "^2PASS^7";
                            }
                            if (cocaine)
                            {
                                cocaineResult = "^8FAIL^7";
                            }
                            else
                            {
                                cocaineResult = "^2PASS^7";
                            }
                            TriggerEvent("chat:addMessage", new
                            {
                                color = drugColour,
                                args = new[] { "[Drugalyser]", "Drugalyser results calculating." }
                            });

                            DrugalyseDelay($"Drugalyser Result: Cannabis: [{cannabisResult}], Cocaine: [{cocaineResult}]");
                        }
                    }
                }
            });

            EventHandlers["Client:RequestBreath"] += new Action<int, int, Vector3>((local, request, location) =>
            {
                if (local == GetPlayerServerId(PlayerId()))
                {
                    var localLocation = GetEntityCoords(PlayerPedId(), true);
                    if (localLocation.DistanceToSquared(location) > 7f)
                    {
                        TriggerServerEvent("Server:ReturnBreath", local, request, 0, false, false);
                    }
                    else
                    {
                        if (awaitingBreath)
                        {
                            TriggerServerEvent("Server:ReturnBreath", local, request, 0, false, false);
                        }
                        else
                        {
                            awaitingBreath = true;
                            requestId = request;
                            breath = 0;

                            TriggerEvent("chat:addMessage", new
                            {
                                color = breathColour,
                                args = new[] { "[Breathalyser]", "You are being breathalysed. Use /setbreath [Amount] or /failprovide." }
                            });
                        }
                    }
                }
            });

            EventHandlers["Client:ReturnBreath"] += new Action<int, int, float, bool, bool>((local, request, breathresult, failprovide, success) =>
            {
                if (request == GetPlayerServerId(PlayerId()))
                {
                    if (success == false)
                    {
                        PlaySoundFrontend(-1, "Place_Prop_Fail", "DLC_Dmod_Prop_Editor_Sounds", false);
                        TriggerEvent("chat:addMessage", new
                        {
                            color = breathColour,
                            args = new[] { "[Breathalyser]", "You are unable to breathalyse this person." }
                        });
                    }
                    else
                    {
                        if (failprovide)
                        {
                            TriggerEvent("chat:addMessage", new
                            {
                                color = breathColour,
                                args = new[] { "[Breathalyser]", "The suspect has failed to provide a specimen of breath." }
                            });
                        }
                        else
                        {
                            TriggerServerEvent("Server:BeepSound", Game.Player.Character.NetworkId, 15.0f, "beep", 0.9f);
                            if (breathresult > bLimit)
                            {
                                TriggerEvent("chat:addMessage", new
                                {
                                    color = breathColour,
                                    args = new[] { "[Breathalyser]", $"Breathalyser ^8FAIL^7: {breathresult} {bUnit}." }
                                });
                            }
                            else
                            {
                                TriggerEvent("chat:addMessage", new
                                {
                                    color = breathColour,
                                    args = new[] { "[Breathalyser]", $"Breathalyser Result: ^2PASS^7: {breathresult} {bUnit}." }
                                });
                            }
                        }
                    }  
                }
            });

            EventHandlers["Client:BeepSound"] += new Action<int, float, string, float>((networkId, soundRadius, soundFile, soundVolume) =>
            {
                Vector3 playerCoords = Game.Player.Character.Position;
                Vector3 targetCoords = GetEntityCoords(NetworkGetEntityFromNetworkId(networkId), true);
                var distance = Vdist(playerCoords.X, playerCoords.Y, playerCoords.Z, targetCoords.X, targetCoords.Y, targetCoords.Z);
                float distanceVolumeMultiplier = (soundVolume / soundRadius);
                float distanceVolume = soundVolume - (distance * distanceVolumeMultiplier);
                if (distance <= soundRadius)
                {
                    SendNuiMessage(string.Format("{{\"submissionType\":\"smartTesting\", \"submissionVolume\":{0}, \"submissionFile\":\"{1}\"}}", (object)distanceVolume, (object)soundFile));
                }
            });

            RegisterCommand("setbreath", new Action<int, List<object>, string>((source, args, raw) =>
            {

                if (IsStringNullOrEmpty(Convert.ToString(args[0])))
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = breathColour,
                        args = new[] { "[Breathalyser]", $"Usage /setbreath [breath]." }
                    });
                }
                else
                {
                    if (awaitingBreath)
                    {
                        float result;
                        var successful = float.TryParse(Convert.ToString(args[0]), out result);
                        if (successful)
                        {
                            breath = result;
                            awaitingBreath = false;
                            TriggerEvent("chat:addMessage", new
                            {
                                color = breathColour,
                                args = new[] { "[Breathalyser]", $"You have blown {result} {dUnit}." }
                            });

                            TriggerServerEvent("Server:ReturnBreath", GetPlayerServerId(PlayerId()), requestId, breath, false, true);
                        }
                        else
                        {
                            PlaySoundFrontend(-1, "Place_Prop_Fail", "DLC_Dmod_Prop_Editor_Sounds", false);
                            TriggerEvent("chat:addMessage", new
                            {
                                color = breathColour,
                                args = new[] { "[Breathalyser]", "Breath specimen invalid." }
                            });
                        }
                    }
                    else
                    {
                        PlaySoundFrontend(-1, "Place_Prop_Fail", "DLC_Dmod_Prop_Editor_Sounds", false);
                        TriggerEvent("chat:addMessage", new
                        {
                            color = breathColour,
                            args = new[] { "[Breathalyser]", $"You are not being breathalysed." }
                        });
                    }    
                }

            }), false);

            RegisterCommand("setdrug", new Action<int, List<object>, string>((source, args, raw) =>
            {

                if (IsStringNullOrEmpty(Convert.ToString(args[0])))
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = drugColour,
                        args = new[] { "[Drugalyser]", $"Usage /setdrug [{d1} (true/false)] [{d2} true/false]." }
                    });
                }
                else
                {
                    if (awaitingDrug)
                    {
                        bool cannabisResult;
                        bool cocaineResult;
                        var successful1 = bool.TryParse(Convert.ToString(args[0]), out cannabisResult);
                        var successful2 = bool.TryParse(Convert.ToString(args[1]), out cocaineResult);
                        if (!successful1 || !successful2)
                        {
                            awaitingBreath = false;
                            TriggerEvent("chat:addMessage", new
                            {
                                color = drugColour,
                                args = new[] { "[Drugalyser]", $"Usage /setdrug [{d1} (true/false)] [{d2} true/false]" }
                            });
                        }
                        else
                        {
                            awaitingDrug = false;
                            TriggerServerEvent("Server:ReturnDrug", GetPlayerServerId(PlayerId()), drugRequestId, cannabisResult, cocaineResult, false, true);
                            TriggerEvent("chat:addMessage", new
                            {
                                color = drugColour,
                                args = new[] { "[Drugalyser]", $"You have provided a saliva sample." }
                            });
                        }
                    }
                    else
                    {
                        PlaySoundFrontend(-1, "Place_Prop_Fail", "DLC_Dmod_Prop_Editor_Sounds", false);
                        TriggerEvent("chat:addMessage", new
                        {
                            color = drugColour,
                            args = new[] { "[Drugalyser]", $"You are not being drugalysed." }
                        });
                    }
                }

            }), false);

            RegisterCommand("breathalyse", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var target = Raycast();
                if (IsEntityAPed(target))
                {
                    var server = GetPlayerServerId(GetPlayer(target));
                    if (!(server == -1))
                    {
                        RequestDictionary("weapons@first_person@aim_rng@generic@projectile@shared@core");
                        TaskPlayAnim(GetPlayerPed(PlayerId()), "weapons@first_person@aim_rng@generic@projectile@shared@core", "idlerng_med", 1.0f, -1, 10000, 50, 0, false, false, false);
                        TriggerEvent("chat:addMessage", new
                        {
                            color = breathColour,
                            args = new[] { "[Breathalyser]", $"You are now breathalysing the suspect." }
                        });
                        TriggerServerEvent("Server:RequestBreath", server, GetPlayerServerId(PlayerId()), GetEntityCoords(PlayerPedId(), true));
                    }
                }
                else
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = breathColour,
                        args = new[] { "[Breathalyser]", $"No player found" }
                    });
                }
            }), false);

            RegisterCommand("drugalyse", new Action<int, List<object>, string>((source, args, raw) =>
            {
                var target = Raycast();
                if (IsEntityAPed(target))
                {
                    var server = GetPlayerServerId(GetPlayer(target));
                    if (!(server == -1))
                    {
                        RequestDictionary("weapons@first_person@aim_rng@generic@projectile@shared@core");
                        TaskPlayAnim(GetPlayerPed(PlayerId()), "weapons@first_person@aim_rng@generic@projectile@shared@core", "idlerng_med", 1.0f, -1, 10000, 50, 0, false, false, false);
                        TriggerEvent("chat:addMessage", new
                        {
                            color = drugColour,
                            args = new[] { "[Drugalyser]", $"You are now drugalysing the suspect." }
                        });
                        TriggerServerEvent("Server:RequestDrug", server, GetPlayerServerId(PlayerId()), GetEntityCoords(PlayerPedId(), true));
                    }
                }
                else
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = drugColour,
                        args = new[] { "[Drugalyser]", $"No player found." }
                    });
                }
            }), false);

            TriggerEvent("chat:addSuggestion", "/setbreath", "Provides a specimen of breath", new[]
            {
                new { name="breath", help="Breath specimen number" },
            });

            TriggerEvent("chat:addSuggestion", "/setdrug", "Provides a saliva sample result", new[]
            {
                new { name=$"{d1}", help="true/false" },
                new { name=$"{d2}", help="true/false" },
            });

            TriggerEvent("chat:addSuggestion", "/breathalyse", "Breathalyse the nearest player");


            TriggerEvent("chat:addSuggestion", "/drugalyse", "Drugalyse the nearest player");

            TriggerEvent("chat:addSuggestion", "/failprovide", "Fail to provide a sample");
        }

        private void ReadConfig()
        {
            var data = LoadResourceFile(GetCurrentResourceName(), "config.ini");
            if (Configuration.LoadFromString(data).Contains("SmartTesting", "breathalyserUnit") == true)
            {
                Configuration loaded = Configuration.LoadFromString(data);
                bUnit = loaded["SmartTesting"]["breathalyserUnit"].StringValue;
                bLimit = loaded["SmartTesting"]["breathalyserLimit"].FloatValue;
                d1 = loaded["SmartTesting"]["drugalyserDrug1"].StringValue;
                d2 = loaded["SmartTesting"]["drugalyserDrug2"].StringValue;
                dDelay = 1000 * loaded["SmartTesting"]["drugalyserDelay"].IntValue;
            }
            else
            {
                ProcessError();
            }
        }
        private void ProcessError()
        {
            PlaySoundFrontend(-1, "ERROR", "HUD_AMMO_SHOP_SOUNDSET", true);
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                multiline = true,
                args = new[] { "[SmartTesting]", $"The config file has not been configured correctly." }
            });
        }

        private async void RequestDictionary(string animDict)
        {
            while(!HasAnimDictLoaded(animDict))
            {
                RequestAnimDict(animDict);
                await Delay(100);
            }
            await Delay(100);
        }

        private async void DrugalyseDelay(string chatmessage)
        {
            await Delay(dDelay);
            TriggerEvent("chat:addMessage", new
            {
                color = drugColour,
                args = new[] { "[Drugalyser]", chatmessage }
            });
        }

        [Command("failprovide")]
        private void FailToProvide()
        {
            if (awaitingBreath)
            {
                awaitingBreath = false;
                TriggerServerEvent("Server:ReturnBreath", GetPlayerServerId(PlayerId()), requestId, 0, true, true);
                TriggerEvent("chat:addMessage", new
                {
                    color = breathColour,
                    args = new[] { "[Breathalyser]", $"You have failed to provide a specimen of breath." }
                });
            }
            else
            {
                if (awaitingDrug)
                {
                    TriggerServerEvent("Server:ReturnDrug", GetPlayerServerId(PlayerId()), drugRequestId, false, false, true, true);
                    awaitingDrug = false;
                }
                else
                {
                    PlaySoundFrontend(-1, "Place_Prop_Fail", "DLC_Dmod_Prop_Editor_Sounds", false);
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 235, 52, 52 },
                        args = new[] { "[System]", $"You are not being breathalysed or drugalysed." }
                    });
                }
            }
        }

        private int Raycast()
        {
            var location = GetEntityCoords(PlayerPedId(), true);
            var offSet = GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0.0f, 2.0f, 0.0f);
            var shapeTest = StartShapeTestCapsule(location.X, location.Y, location.Z, offSet.X, offSet.Y, offSet.Z, 1.0f, 12, PlayerPedId(), 7);
            bool hit = false;
            Vector3 endCoords = new Vector3(0f, 0f, 0f);
            Vector3 surfaceNormal = new Vector3(0f, 0f, 0f);
            int entityHit = 0;
            var result = GetShapeTestResult(shapeTest, ref hit, ref endCoords, ref surfaceNormal, ref entityHit);
            return entityHit;
        }

        private int GetPlayer(int player)
        {
            for (int i = 0; i < playerLoop; i++)
            {
                if (GetPlayerPed(i) == player)
                {
                    return i;
                }
            }
            return -1;
        } 
    }
}
