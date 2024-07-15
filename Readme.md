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
