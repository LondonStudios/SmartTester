using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class ResultTransfer : BaseScript
    {
        public ResultTransfer()
        {
            EventHandlers["Server:RequestBreath"] += new Action<int, int, Vector3>((local, request, location) =>
            {
                TriggerClientEvent("Client:RequestBreath", local, request, location);
            });

            EventHandlers["Server:ReturnBreath"] += new Action<int, int, float, bool, bool>((local, request, breath, failprovide, success) =>
            {
                TriggerClientEvent("Client:ReturnBreath", local, request, breath, failprovide, success);
            });

            EventHandlers["Server:RequestDrug"] += new Action<int, int, Vector3>((local, request, location) =>
            {
                TriggerClientEvent("Client:RequestDrug", local, request, location);
            });

            EventHandlers["Server:ReturnDrug"] += new Action<int, int, bool, bool, bool, bool>((local, request, cannabis, cocaine, failprovide, success) =>
            {
                TriggerClientEvent("Client:ReturnDrug", local, request, cannabis, cocaine, failprovide, success);
            });

            EventHandlers["Server:BeepSound"] += new Action<int, float, string, float>((networkId, soundRadius, soundFile, soundVolume) =>
            {
                TriggerClientEvent("Client:BeepSound", networkId, soundRadius, soundFile, soundVolume);
            });
        }
    }
}
