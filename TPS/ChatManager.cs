using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ChatManager : MonoBehaviour {
    
    public int maxMessages = 25;

    public GameObject chatPanel;
    public GameObject textObject;    
    public Color playerMessage, combatLog, combatNotice, combatFlag;

    [SerializeField]
	List<Message> messageList = new List<Message>();
    
    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        // Remove the oldest message when max is reached.
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        // Create the Message
        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        newMessage.textObject.color = MessageTypeColor(messageType);

        // Add to the Message List
        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = playerMessage;

        switch (messageType)
        {
            case Message.MessageType.combatLog:
                color = combatLog; 
                break;

            case Message.MessageType.combatNotice:
                color = combatNotice;
                break;

            case Message.MessageType.combatFlag:
                color = combatFlag;
                break;
        
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        combatLog,
        combatNotice,
        combatFlag
    }
}
