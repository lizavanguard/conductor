using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.MessageBus
{
    public class ActorWalkBus
    {
        Dictionary<int, Action<Message>> handlerContainer;
        List<AddressedMessage> messageQueue;

        public struct AddressedMessage
        {
            public int Address { get; private set; }
            public Message Message { get; private set; }

            public AddressedMessage(int address, Message message)
            {
                Address = address;
                Message = message;
            }
        }

        public struct Message
        {
            public bool Front { get; private set; }

            public Message(bool front)
            {
                this.Front = front;
            }
        }

        public ActorWalkBus()
        {
            handlerContainer = new Dictionary<int, Action<Message>>();
            messageQueue = new List<AddressedMessage>();
        }

        public int Connect(int address, Action<Message> handler)
        {
            handlerContainer.Add(address, handler);

            return address;
        }

        public void Disconnect(int address)
        {
            if (handlerContainer.ContainsKey(address))
            {
                handlerContainer.Remove(address);
            }
        }

        public void SendMessage(int address, Message message)
        {
            messageQueue.Add(new AddressedMessage(address, message));
        }

        public void Dispatch()
        {
            foreach (var addressedMessage in messageQueue)
            {
                if (handlerContainer.ContainsKey(addressedMessage.Address))
                {
                    var handler = handlerContainer[addressedMessage.Address];
                    handler(addressedMessage.Message);
                }
            }
            messageQueue.Clear();
        }
    }
}
