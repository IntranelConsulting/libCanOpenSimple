/*
    This file is part of libCanopenSimple.
    libCanopenSimple is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    libCanopenSimple is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    You should have received a copy of the GNU General Public License
    along with libCanopenSimple.  If not, see <http://www.gnu.org/licenses/>.
 
    Copyright(c) 2017 Robin Cornelius <robin.cornelius@gmail.com>
*/


using System;
using System.Runtime.InteropServices;

namespace libCanopenSimple
{
    /// <summary>
    /// CanFestival message packet. Note we set data to be a UInt64 as inside canfestival its a fixed char[8] array
    /// we cannout use fixed arrays in C# without UNSAFE so instead we just use a UInt64
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 12, Pack = 1)]
    public struct Message
    {
        public UInt16 cob_source_id; /**< message's ID */
        public UInt16 cob_id; /**< message's ID */
        public byte rtr;       /**< remote transmission request. (0 if not rtr message, 1 if rtr message) */
        public byte len;       /**< message's length (0 to 8) */
        public UInt64 data;
    }

    /// <summary>
    /// CANOpen message recieved callback, this will be fired upon any recieved complete message on the bus
    /// </summary>
    /// <param name="msg">The CanOpen message</param>
    public delegate void RxMessage(Message msg, bool bridge = false);
    
    public interface IDriverInstance
    {
        event RxMessage rxmessage;

        Message canreceive();
        void cansend(Message msg);
        void close();
        void enumerate();
        bool isOpen();

        /// <summary>
        /// Open a connection to a particular CAN bus
        /// </summary>
        /// <param name="bus">Something like can0 or vcan0 on linux</param>
        /// <param name="speed">Note that speed is preconfigured for SocketCan and can not be changed - this parameter is ignored</param>
        /// <returns></returns>
        bool open(string bus, BUSSPEED speed);
    }
}