using BackEnd.Tcp;
using UnityEngine;
using System.Collections.Generic;

namespace Protocol
{
    // 이벤트 타입
    public enum Type : sbyte
    {
        Key = 0,        // 키(가상 조이스틱) 입력
        PlayerMove,     // 플레이어 이동
        PlayerChat      // 플레이어 채팅
    }


    public class Message
    {
        public Type type;

        public Message(Type type)
        {
            this.type = type;
        }
    }

    public class PlayerMoveMessage : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;
        public float zPos;
        public float xDir;
        public float yDir;
        public float zDir;
        public PlayerMoveMessage(SessionId session, Vector3 pos, Vector3 dir) : base(Type.PlayerMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
            this.zPos = pos.z;
            this.xDir = dir.x;
            this.yDir = dir.y;
            this.zDir = dir.z;
        }
    }

    public class PlayerChatMessage : Message
    {
        public SessionId playerSession;
        public string chat;

        public PlayerChatMessage(SessionId session, string chat) : base(Type.PlayerChat)
        {
            this.playerSession = session;
            this.chat = chat;
        }
    }
}
