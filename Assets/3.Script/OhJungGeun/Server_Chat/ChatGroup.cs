using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatGroup
{
    public string groupName;
    public string alias;
    public string inDate;
    public int maxUserCount;
    public int joinedUserCount;
    public string serverAddress;
    public ushort serverPort;

    public override string ToString()
    {
        return $"groupName : {groupName}\n" +
        $"alias : {alias}\n" +
        $"inDate : {inDate}\n" +
        $"maxUserCount : {maxUserCount}\n" +
        $"joinedUserCount : {joinedUserCount}\n" +
        $"serverAddress : {serverAddress}\n" +
        $"serverPort : {serverPort}\n";
    }
}
