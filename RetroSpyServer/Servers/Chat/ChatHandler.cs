﻿using System;
using System.Collections.Generic;
using System.Text;
using RetroSpyServer.DBQueries;
using static GameSpyLib.Common.Random;

namespace RetroSpyServer.Servers.PeerChat
{
    public class ChatHandler
    {

        public static ChatDBQuery DBQuery = null;

        public static void Crypt(ChatClient client, string[] recieved)
        {
            string gameName = recieved[3].Substring(0,recieved[3].Length-2);
            string serverKey = GenerateRandomString(17, StringType.AlphaNumeric);
            string clientKey = GenerateRandomString(17, StringType.AlphaNumeric); 
            string sendingBuffer = string.Format(":s {0} * {1}{2}\r\n", 705,serverKey,clientKey);
            client.Stream.SendAsync(sendingBuffer);
        }

        internal static void WallOps(ChatClient chatClient, string[] recieved)
        {
            throw new NotImplementedException();
        }

        internal static void User(ChatClient chatClient, string[] recieved)
        {
            throw new NotImplementedException();
        }
    }
}