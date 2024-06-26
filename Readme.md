Creating a chatbot using Rasa that can run queries on DynamoDB to give users specific information involves several steps. Below is a detailed guide from setting up the environment to creating a working chatbot that can handle queries like fetching the last 5 transactions of a particular user.rpy

### Prerequisites

1. **Python** installed (3.7 or higher).
2. **AWS account** with DynamoDB setup.
3. **AWS CLI** configured with appropriate permissions.
4. **Rasa** installed.
5. **Boto3** library for AWS DynamoDB integration.

### Step-by-Step Guide

#### 1. **Setting Up the Environment**

1. **Install Rasa**:
   ```bash
   pip install rasa
   ```

2. **Install Boto3**:
   ```bash
   pip install boto3
   ```

3. **Create a new Rasa project**:
   ```bash
   rasa init
   ```

#### 2. **Define Your NLU Data**

Edit `data/nlu.yml` to include intents for querying transactions:
```yaml
version: "3.1"

nlu:
- intent: greet
  examples: |
    - hello
    - hi
    - hey

- intent: goodbye
  examples: |
    - bye
    - goodbye

- intent: get_last_transactions
  examples: |
    - show me the last 5 transactions
    - get the latest transactions
    - what are my recent transactions
```

#### 3. **Define Your Domain**

Edit `domain.yml` to define the entities, slots, and responses:
```yaml
version: "3.1"

intents:
  - greet
  - goodbye
  - get_last_transactions

entities:
  - user_id

slots:
  user_id:
    type: text

responses:
  utter_greet:
    - text: "Hello! How can I assist you today?"

  utter_goodbye:
    - text: "Goodbye!"

  utter_ask_user_id:
    - text: "Please provide your user ID."

  utter_show_transactions:
    - text: "Here are your last 5 transactions: {transactions}"
```

#### 4. **Define Your Stories**

Edit `data/stories.yml` to define how conversations should flow:
```yaml
version: "3.1"

stories:
- story: greet path
  steps:
    - intent: greet
    - action: utter_greet

- story: goodbye path
  steps:
    - intent: goodbye
    - action: utter_goodbye

- story: get transactions path
  steps:
    - intent: get_last_transactions
    - action: action_get_last_transactions
```

#### 5. **Create Custom Actions**

Edit `actions/actions.py` to define custom actions to query DynamoDB:
```python
import boto3
from typing import Any, Text, Dict, List
from rasa_sdk import Action, Tracker
from rasa_sdk.executor import CollectingDispatcher

# Initialize DynamoDB client
dynamodb = boto3.client('dynamodb', region_name='us-east-1')

class ActionGetLastTransactions(Action):

    def name(self) -> Text:
        return "action_get_last_transactions"

    def run(self, dispatcher: CollectingDispatcher, tracker: Tracker, domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:
        user_id = tracker.get_slot('user_id')
        
        if not user_id:
            dispatcher.utter_message(text="I need your user ID to fetch transactions.")
            return []

        # Query DynamoDB to get the last 5 transactions
        response = dynamodb.query(
            TableName='TransactionsTable',
            KeyConditionExpression='user_id = :uid',
            ExpressionAttributeValues={':uid': {'S': user_id}},
            Limit=5,
            ScanIndexForward=False  # Get the most recent transactions
        )

        transactions = response.get('Items', [])
        transactions_list = [t['transaction_id']['S'] for t in transactions]

        dispatcher.utter_message(text=f"Here are your last 5 transactions: {transactions_list}")
        return []

```

#### 6. **Update Your Endpoint Configuration**

Edit `endpoints.yml` to enable custom actions:
```yaml
action_endpoint:
  url: "http://localhost:5055/webhook"
```

#### 7. **Run the Action Server**

In a separate terminal, start the action server:
```bash
rasa run actions
```

#### 8. **Train the Model**

Train your Rasa model:
```bash
rasa train
```

#### 9. **Run the Rasa Server**

Start your Rasa server:
```bash
rasa run
```

#### 10. **Test Your Chatbot**

Interact with your chatbot using Rasa Shell:
```bash
rasa shell
```

You can now test the conversation flow:
```
User: hi
Bot: Hello! How can I assist you today?
User: show me the last 5 transactions
Bot: I need your user ID to fetch transactions.
User: my user ID is 12345
Bot: Here are your last 5 transactions: [...]
```

### Conclusion

This is a basic implementation of a Rasa chatbot that interacts with DynamoDB to fetch and display the last 5 transactions for a user. You can extend this model by adding more intents, entities, and custom actions as needed for your application.

sk-tlngWE7Bg9ibsrrrU1qNT3BlbkFJNXwAXmLoBxyFHQTcw
