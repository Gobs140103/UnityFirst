<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Chatbot</title>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <h1>Chatbot</h1>
    <div id="chatbox"></div>
    <input type="text" id="userInput" placeholder="Type your message here..." />
    <button onclick="sendMessage()">Send</button>

    <script>
        async function sendMessage() {
            const message = document.getElementById('userInput').value;
            const token = localStorage.getItem('token');
            const response = await fetch('http://localhost:5005/webhooks/rest/webhook', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ sender: 'user', message: message })
            });
            const data = await response.json();
            displayMessage(data);
        }

        function displayMessage(data) {
            const chatbox = document.getElementById('chatbox');
            data.forEach(message => {
                const msg = document.createElement('div');
                msg.textContent = message.text;
                chatbox.appendChild(msg);
            });
        }
    </script>
</body>
</html>


