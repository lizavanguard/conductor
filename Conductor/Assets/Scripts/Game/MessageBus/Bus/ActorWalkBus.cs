using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Consuctor.Game.MessageBus
{
    public class ActorWalkBus
    {
        int nextId;
        Dictionary<int, Action<Message>> handlerContainer;
        List<Message> messageQueue;

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
            nextId = 0;
            handlerContainer = new Dictionary<int, Action<Message>>();
            messageQueue = new List<Message>();
        }

        public int Connect(Action<Message> handler)
        {
            int id = nextId;
            handlerContainer.Add(id, handler);
            nextId++;

            return id;
        }

        public void Disconnect(int id)
        {
            if (handlerContainer.ContainsKey(id))
            {
                handlerContainer.Remove(id);
            }
        }

        public void SendMessage(Message message)
        {
            messageQueue.Add(message);
        }

        public void Dispatch()
        {
            foreach (var message in messageQueue)
            {
                foreach (var handler in handlerContainer.Values)
                {
                    handler.Invoke(message);
                }
            }

            messageQueue.Clear();
        }
    }
}
