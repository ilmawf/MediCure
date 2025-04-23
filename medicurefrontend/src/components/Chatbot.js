// components/Chatbot.js
import React, { useState } from 'react';
import { Link } from 'react-router-dom';

const Chatbot = () => {
  const [isOpen, setIsOpen] = useState(false);  // Track if the chat window is open

  // Toggle chat window visibility
  const toggleChatWindow = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div className="chatbot-container">
      {/* Chatbot Button */}
      {!isOpen && (
        <button className="chatbot-btn" onClick={toggleChatWindow}>
          {/* Use an icon here (you can add an SVG or a font icon) */}
          <img src={require('../assets/chat-icon.png')} alt="Chat Icon" className="chat-icon" />
          Let's Chat
        </button>
      )}

      {/* Chat Window */}
      {isOpen && (
        <div className="chat-window">
          <div className="chat-header">
            <h3>Virtual Assistant</h3>
            <button className="close-btn" onClick={toggleChatWindow}>X</button>
          </div>
          <div className="chat-body">
            <p><strong>Assistant:</strong> Hello! How can I assist you today?</p>
            <input type="text" placeholder="Type your question here..." />
            <button className="send-btn">Send</button>
          </div>
        </div>
      )}
    </div>
  );
};

export default Chatbot;
