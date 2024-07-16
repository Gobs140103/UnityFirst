Yes, you do need to define NLU training data to recognize the intent and entities from the user messages. Let's include that part as well. Here’s a complete setup:

### 1. Define Slots in Rasa

First, define the slots in your `domain.yml` file to store the attributes.

```yaml
slots:
  attribute_1:
    type: bool
    initial_value: false
  attribute_2:
    type: bool
    initial_value: false
  # Add more attributes as needed
```

### 2. Create Custom Actions

In your `actions.py` file, create a custom action to handle the attributes.

```python
import json
from rasa_sdk import Action, Tracker
from rasa_sdk.executor import CollectingDispatcher
from rasa_sdk.events import SlotSet

class ActionSetAttributes(Action):
    def name(self):
        return "action_set_attributes"

    def run(self, dispatcher: CollectingDispatcher, tracker: Tracker, domain: dict):
        latest_message = tracker.latest_message['text']
        attributes = json.loads(latest_message)
        
        attribute_1 = attributes.get('attribute_1', False)
        attribute_2 = attributes.get('attribute_2', False)
        
        # Process the attributes and perform the query
        dispatcher.utter_message(text=f"Querying database with attributes: attribute_1={attribute_1}, attribute_2={attribute_2}")
        
        return [SlotSet("attribute_1", attribute_1), SlotSet("attribute_2", attribute_2)]
```

### 3. Define Intents and Responses

Define an intent for setting attributes and responses for acknowledgment.

**Intents and Responses in `domain.yml`**

```yaml
intents:
  - set_attributes

responses:
  utter_acknowledge_attributes:
    - text: "Attributes received. Processing your query..."
```

**NLU Training Data in `nlu.yml`**

```yaml
nlu:
- intent: set_attributes
  examples: |
    - Set attributes
    - Here are the attributes
    - These are the attributes
```

### 4. Update Stories

Update your stories to include the custom action.

```yaml
stories:
- story: set attributes and query
  steps:
  - intent: set_attributes
  - action: action_set_attributes
  - action: utter_acknowledge_attributes
```

### 5. Create the Front End with HTML, CSS, and JavaScript

Create a simple HTML form with checkboxes to select the attributes and a script to send the data to your Rasa bot.

**HTML**

```html
<!DOCTYPE html>
<html>
<head>
    <title>Attribute Form</title>
    <style>
        body {
            font-family: Arial, sans-serif;
        }
        form {
            max-width: 300px;
            margin: 50px auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 10px;
        }
        label {
            display: block;
            margin-bottom: 10px;
        }
        button {
            margin-top: 20px;
            padding: 10px 20px;
            background-color: #007BFF;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }
    </style>
</head>
<body>

<form id="attributeForm">
    <label>
        Attribute 1:
        <input type="checkbox" name="attribute_1">
    </label>
    <label>
        Attribute 2:
        <input type="checkbox" name="attribute_2">
    </label>
    <button type="button" onclick="submitForm()">Submit</button>
</form>

<script>
    async function submitForm() {
        const form = document.getElementById('attributeForm');
        const formData = new FormData(form);
        const attributes = {};

        formData.forEach((value, key) => {
            attributes[key] = value === 'on';
        });

        const response = await fetch('http://localhost:5005/webhooks/rest/webhook', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                sender: 'user',
                message: JSON.stringify(attributes)
            }),
        });

        const data = await response.json();
        console.log(data);
    }
</script>

</body>
</html>
```

### Summary

1. Define slots for attributes in `domain.yml`.
2. Create custom actions in `actions.py` to handle attributes and perform queries.
3. Define intents and responses for setting attributes in `domain.yml`.
4. Create NLU training data in `nlu.yml` to recognize the intent.
5. Update stories to include custom actions in `stories.yml`.
6. Create a simple HTML form with checkboxes and a JavaScript function to send the selected attributes to the Rasa bot.

This setup allows users to select multiple attributes via checkboxes in a web form. When the user submits the form, the selected attributes are sent to the Rasa bot, which processes the input and performs the necessary query.

To implement checkboxes for attribute selection in a non-React environment with your existing setup, you can use plain HTML, CSS, and JavaScript. Here's how you can achieve this:

1. **Update your `index.html` to include the checkboxes**:
   Add a section for the checkboxes inside your chatbot widget.

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
               <div id="attribute-selector" style="display: none;">
                   <h5>Select attributes to query:</h5>
                   <label>
                       <input type="checkbox" id="txn_nb" value="txn_nb" />
                       <span>Transaction Number</span>
                   </label>
                   <label>
                       <input type="checkbox" id="txn_id" value="txn_id" />
                       <span>Transaction ID</span>
                   </label>
                   <!-- Add more checkboxes for other attributes as needed -->
                   <button id="submit-attributes">Submit</button>
               </div>
           </div>

           <!-- Bot profile -->
           <div class="profile_div" id="profile_div">
               <img class="imgProfile" src="static/img/botAvatar.png" alt="Bot Avatar">
           </div>
       </div>

       <!-- Scripts -->
       <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
       <script src="static/js/lib/materialize.min.js"></script>
       <script src="static/js/lib/uuid.min.js"></script>
       <script src="static/js/script.js"></script>
       <script src="static/js/lib/chart.min.js"></script>
       <script src="static/js/lib/showdown.min.js"></script>
       <script>
           $(document).ready(function() {
               // Show the attribute selector when needed
               function showAttributeSelector() {
                   $("#attribute-selector").show();
               }

               // Hide the attribute selector
               function hideAttributeSelector() {
                   $("#attribute-selector").hide();
               }

               // Handle the attribute submission
               $("#submit-attributes").click(function() {
                   let selectedAttributes = [];
                   $("#attribute-selector input:checked").each(function() {
                       selectedAttributes.push($(this).val());
                   });
                   hideAttributeSelector();
                   sendSelectedAttributesToRasa(selectedAttributes);
               });

               // Function to send selected attributes to Rasa
               function sendSelectedAttributesToRasa(attributes) {
                   attributes.forEach(attribute => {
                       let payload = `/select_attribute{"attribute": "${attribute}"}`;
                       sendMessageToRasa(payload);
                   });
               }

               // Function to send message to Rasa
               function sendMessageToRasa(message) {
                   // Implement the logic to send message to Rasa
               }

               // Show the attribute selector (for demo purposes)
               showAttributeSelector();
           });
       </script>
   </body>
   </html>
   ```

2. **Create a custom action in your Rasa bot to handle the attribute selection**:
   Make sure your Rasa bot is set up to handle the `/select_attribute` intent and update the `actions.py` file to capture the attributes.

   ```python
   from typing import Any, Text, Dict, List
   from rasa_sdk import Action, Tracker
   from rasa_sdk.executor import CollectingDispatcher
   from rasa_sdk.events import SlotSet

   class ActionCaptureAttribute(Action):
       def name(self) -> Text:
           return "action_capture_attribute"

       def run(self, dispatcher: CollectingDispatcher,
               tracker: Tracker,
               domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

           attribute = tracker.get_slot("attribute")

           if attribute:
               if attribute == "txn_nb":
                   return [SlotSet("txn_nb", True)]
               elif attribute == "txn_id":
                   return [SlotSet("txn_id", True)]
               # Add more conditions for other attributes

           return []

   class ActionAskForAttributes(Action):
       def name(self) -> Text:
           return "action_ask_for_attributes"

       def run(self, dispatcher: CollectingDispatcher,
               tracker: Tracker,
               domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:

           dispatcher.utter_message(text="Please select the attributes you want to query.")
           return []
   ```

3. **Update your `domain.yml` to include the slots and intents**:
   ```yaml
   intents:
     - select_attribute

   slots:
     txn_nb:
       type: text
       influence_conversation: false
     txn_id:
       type: text
       influence_conversation: false
     # Add other attributes

   actions:
     - action_capture_attribute
     - action_ask_for_attributes
   ```

4. **Define the intent and response in your `nlu.yml` and `responses.yml`**:
   ```yaml
   nlu:
   - intent: select_attribute
     examples: |
       - I want to see transaction number
       - Show me the transaction ID

   responses:
     utter_ask_for_attributes:
       - text: "Please select the attributes you want to query."
   ```

5. **Add the rules or stories in your `rules.yml` or `stories.yml`**:
   ```yaml
   rules:
   - rule: Capture user attribute selection
     steps:
     - intent: select_attribute
     - action: action_capture_attribute
   ```

By following these steps, you can integrate checkboxes for attribute selection in your Rasa chatbot without using React. The JavaScript in your `index.html` will handle the user interaction, and the custom actions in Rasa will process the selected attributes.
