To integrate the custom query chatbot (with checkboxes) into your existing setup using only HTML, CSS, and JavaScript, you can update your `index.html` and `style.css` accordingly. Here’s how you can do it:

### Updated `index.html`

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Chatbot Widget</title>

    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans&family=Raleway:500&family=Roboto:wght@300&family=Lato&display=swap" rel="stylesheet">

    <!-- Font Awesome Icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" integrity="sha256-eZrrJcwDc/3uDhsdt61sL2oOBY362qM3lon1gyExkL0=" crossorigin="anonymous">

    <!-- Materialize CSS -->
    <link rel="stylesheet" href="static/css/materialize.min.css">

    <!-- Main CSS -->
    <link rel="stylesheet" href="static/css/style.css">
</head>
<body>
    <div class="container">
        <!-- Modal for rendering charts -->
        <div id="modal1" class="modal">
            <canvas id="modal-chart"></canvas>
        </div>

        <!-- Chatbot widget -->
        <div class="widget">
            <div class="chat_header">
                <span class="chat_header_title">Sara</span>
                <span class="dropdown-trigger" href="#" data-target="dropdown1">
                    <i class="material-icons">more_vert</i>
                </span>
                <!-- Dropdown menu -->
                <ul id="dropdown1" class="dropdown-content">
                    <li><a href="#" id="clear">Clear</a></li>
                    <li><a href="#" id="restart">Restart</a></li>
                    <li><a href="#" id="close">Close</a></li>
                </ul>
            </div>
            <div class="chats" id="chats">
                <div class="clearfix"></div>
            </div>
            <div class="keypad">
                <textarea id="userInput" placeholder="Type a message..." class="usrInput"></textarea>
                <div id="sendButton" role="button" aria-label="Send message">
                    <i class="fa fa-paper-plane" aria-hidden="true"></i>
                </div>
            </div>
        </div>

        <!-- Bot profile -->
        <div class="profile_div" id="profile_div">
            <img class="imgProfile" src="static/img/botAvatar.png" alt="Bot Avatar">
        </div>
    </div>

    <!-- Checkbox form -->
    <div id="checkboxForm" style="display:none;">
        <form id="customQueryForm">
            <label>
                <input type="checkbox" name="attribute" value="name" />
                <span>Name</span>
            </label>
            <label>
                <input type="checkbox" name="attribute" value="email" />
                <span>Email</span>
            </label>
            <label>
                <input type="checkbox" name="attribute" value="phone" />
                <span>Phone</span>
            </label>
            <label>
                <input type="checkbox" name="attribute" value="address" />
                <span>Address</span>
            </label>
            <button type="button" onclick="submitCustomQuery()">Submit</button>
        </form>
    </div>

    <!-- Scripts -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="static/js/lib/materialize.min.js"></script>
    <script src="static/js/lib/uuid.min.js"></script>
    <script src="static/js/script.js"></script>
    <script src="static/js/lib/chart.min.js"></script>
    <script src="static/js/lib/showdown.min.js"></script>

    <script>
        document.getElementById('sendButton').addEventListener('click', function() {
            const message = document.getElementById('userInput').value;
            if (message.toLowerCase() === 'custom query') {
                document.getElementById('checkboxForm').style.display = 'block';
            } else {
                sendMessage(message);
            }
        });

        function sendMessage(message) {
            const payload = {
                sender: "user",
                message: message
            };
            
            fetch('http://localhost:5005/webhooks/rest/webhook', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(payload)
            })
            .then(response => response.json())
            .then(data => {
                // Handle the response from Rasa
                console.log(data);
                // Clear the input box
                document.getElementById('userInput').value = '';
            })
            .catch(error => {
                console.error('Error:', error);
            });
        }

        function submitCustomQuery() {
            const form = document.getElementById('customQueryForm');
            const formData = new FormData(form);
            const selectedAttributes = [];
            formData.forEach((value, key) => {
                selectedAttributes.push(value);
            });

            const queryMessage = `Custom query for: ${selectedAttributes.join(', ')}`;
            sendMessage(queryMessage);

            // Hide the form after submission
            document.getElementById('checkboxForm').style.display = 'none';
        }
    </script>
</body>
</html>
```

### Updated `style.css`

```css
body {
  font-family: "Open Sans", sans-serif;
}

.widget {
  display: none;
  width: 350px;
  right: 15px;
  height: 500px;
  bottom: 5%;
  position: fixed;
  background: #f9f9f9;
  border-radius: 10px;
  box-shadow: 0 0 1px rgba(0, 0, 0, 0.16), 0 0 5px rgba(0, 0, 0, 0.15);
}

.chat_header {
  height: 60px;
  background: #2c3e50;
  border-radius: 10px 10px 0 0;
  padding: 5px;
  font-size: 20px;
}

.chat_header_title {
  color: white;
  float: left;
  margin-top: 3%;
  margin-left: 35%;
}

.chats {
  height: 385px;
  padding: 2px;
  border-radius: 1px;
  overflow-y: scroll;
  margin-top: 1px;
  transition: 0.2s;
}

div.chats::-webkit-scrollbar {
  overflow-y: hidden;
  width: 0;
}

.clearfix {
  margin-top: 2px;
  margin-bottom: 2px;
}

.botAvatar {
  border-radius: 50%;
  width: 1.5em;
  height: 1.5em;
  float: left;
  margin-left: 5px;
}

.botMsg {
  float: left;
  margin-top: 5px;
  background: #ecf0f1;
  color: black;
  margin-left: 0.5em;
  padding: 10px;
  border-radius: 1.5em;
  max-width: 60%;
  min-width: 25%;
  font-size: 13px;
  word-wrap: break-word;
  border-radius: 5px 20px 20px 20px;
}

div.chats > pre {
  font-family: monospace, monospace;
  font-size: 1em;
  margin: 0 !important;
  white-space: initial !important;
}
div.chats > ul:not(.browser-default) {
  padding-left: 1em;
}
div.chats > ul:not(.browser-default) > li {
  list-style-type: initial;
}

div.chats > span.botMsg > ol {
  padding-left: 1em;
}

textarea {
  box-shadow: none;
  resize: none;
  outline: none;
  overflow: hidden;
  font-family: Lato;
}

textarea::-webkit-input-placeholder {
  font-family: Lato;
}

textarea-webkit-scrollbar {
  width: 0 !important;
}

.userMsg {
  animation: animateElement linear 0.2s;
  animation-iteration-count: 1;
  margin-top: 5px;
  word-wrap: break-word;
  padding: 10px;
  float: right;
  margin-right: 0.5em;
  background: #2c3e50;
  color: white;
  margin-bottom: 0.15em;
  font-size: 13px;
  max-width: 65%;
  min-width: 15%;
  border-radius: 20px 5px 20px 20px;
}

blockquote {
  margin: 20px 0;
  padding-left: 1.5rem;
  border-left: 5px solid #2c3e50;
  margin-block-start: 0 !important;
  margin-block-end: 0 !important;
  margin-inline-start: 0

 !important;
  margin-inline-end: 0 !important;
}

.userAvatar {
  animation: animateElement linear 0.3s;
  animation-iteration-count: 1;
  border-radius: 50%;
  width: 1.5em;
  height: 1.5em;
  float: right;
  margin-right: 5px;
}

.usrInput {
  padding: 0.5em;
  width: 80%;
  margin-left: 4%;
  border: 0;
  padding-left: 15px;
  height: 40px;
}

.keypad {
  background: #f9f9f9;
  height: 50px;
  position: absolute;
  bottom: 0;
  width: 100%;
  padding: 5px;
  border-radius: 0 0 10px 10px;
  border-top: 1px solid #ecf0f1;
}

#sendButton {
  height: 20px;
  width: 20px;
  border-radius: 50%;
  display: inline-block;
  float: right;
  margin-right: 30px;
  text-align: center;
  padding: 5px;
  font-size: 20px;
  cursor: pointer;
  color: #2c3e50;
}

.imgProfile {
  box-shadow: 0 0 1px rgba(0, 0, 0, 0.16), 0 0 10px rgba(0, 0, 0, 0.15);
  border-radius: 50px;
  width: 60%;
}

.profile_div {
  position: fixed;
  padding: 5px;
  width: 10%;
  bottom: 0;
  right: 0;
  cursor: pointer;
  animation: bounceBot 1s infinite;
}
  
@keyframes bounceBot {
  0%, 100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-30px);
  }
}
  
#close,
#restart,
#clear {
  cursor: pointer;
}

.dropdown-trigger {
  cursor: pointer;
  color: white;
  margin-right: 5px;
  float: right;
  margin-top: 3%;
}

.dropdown-content li > a,
.dropdown-content li > span {
  color: #2c3e50;
}

@keyframes animateElement {
  0% {
    opacity: 0;
    transform: translate(0px, 10px);
  }
  100% {
    opacity: 1;
    transform: translate(0px, 0px);
  }
}

.suggestions {
  padding: 5px;
  width: 80%;
  border-radius: 10px;
  background: #ffffff;
  box-shadow: 2px 5px 5px 1px #dbdade;
}

.menuTitle {
  padding: 5px;
  margin-top: 5px;
  margin-bottom: 5px;
}

.menu {
  padding: 5px;
}

.menuChips {
  display: block;
  background: #2c3e50;
  color: #fff;
  text-align: center;
  padding: 5px;
  margin-bottom: 5px;
  cursor: pointer;
  border-radius: 15px;
  font-size: 14px;
  word-wrap: break-word;
}

#expand:hover {
  font-size: 18px;
}

#expand {
  position: absolute;
  right: 10px;
  top: 10px;
}

.modal {
  height: 60%;
  border-radius: 10px;
}

.chart-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin: auto;
  width: 90%;
  max-width: 100%;
  border-radius: 10px;
  background: white;
  box-shadow: 2px 3px 9px rgba(0, 0, 0, 0.1);
  margin-top: 5px;
}

.botTyping {
  float: left;
  margin-top: 5px;
  background: #ecf0f1;
  color: #000000;
  margin-left: 0.5em;
  padding: 15px;
  border-radius: 5px 20px 20px 20px;
  max-width: 60%;
  min-width: 20%;
  word-wrap: break-word;
  border-radius: 5px 20px 20px 20px;
}

.botTyping > div {
  width: 10px;
  height: 10px;
  background-color: #2c3e50;
  border-radius: 100%;
  display: inline-block;
  animation: bounce 1.2s infinite;
}

.botTyping .bounce1 {
  animation-delay: 0s;
}

.botTyping .bounce2 {
  animation-delay: 0.2s;
}

.botTyping .bounce3 {
  animation-delay: 0.4s;
}

@keyframes bounce {
  0%, 100% {
    transform: scale(0);
  }
  50% {
    transform: scale(1);
  }
}

.tap-target {
  color: #fff;
  background: #2c3e50;
}
```

In this integration, the checkbox form is displayed when the user types "custom query". The form allows the user to select attributes they want to query, and upon submission, the selected attributes are sent as a message to Rasa. The CSS is updated to ensure proper styling of the checkbox form.
