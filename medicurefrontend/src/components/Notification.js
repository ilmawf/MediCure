import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

const Notification = () => {
  const [messages, setMessages] = useState([]);
  const [connection, setConnection] = useState(null);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/notificationHub')  // Your SignalR Hub URL
      .build();

    newConnection
      .start()
      .then(() => console.log('SignalR Connected'))
      .catch((err) => console.log('Error while establishing connection: ', err));

    newConnection.on('ReceiveMessage', (message) => {
      setMessages((prevMessages) => [...prevMessages, message]);
    });

    setConnection(newConnection);
  }, []);

  return (
    <div>
      <h2>Real-Time Notifications</h2>
      <ul>
        {messages.map((message, index) => (
          <li key={index}>{message}</li>
        ))}
      </ul>
    </div>
  );
};

export default Notification;
