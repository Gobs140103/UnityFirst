To extend the chatbot to handle queries for the last 5 transactions of a specific user, you will need to modify your DynamoDB table structure and your Python script accordingly.

### Step 1: Modify DynamoDB Table Structure

First, create a table that can store user transactions. Let's name it `UserTransactions` with `user_id` as the partition key and `transaction_id` as the sort key. This allows you to store multiple transactions per user and query them in order.

1. **Create the DynamoDB table**:

    - Go to the AWS Management Console.
    - Navigate to DynamoDB.
    - Create a table named `UserTransactions` with:
        - `user_id` as the partition key (string).
        - `transaction_id` as the sort key (number).

2. **Insert sample transactions**:

    You can use the AWS CLI to insert sample transactions. Here is an example:

    ```bash
    aws dynamodb put-item --table-name UserTransactions --item '{"user_id": {"S": "1"}, "transaction_id": {"N": "1"}, "amount": {"N": "100"}, "description": {"S": "Transaction 1"}}'
    aws dynamodb put-item --table-name UserTransactions --item '{"user_id": {"S": "1"}, "transaction_id": {"N": "2"}, "amount": {"N": "200"}, "description": {"S": "Transaction 2"}}'
    aws dynamodb put-item --table-name UserTransactions --item '{"user_id": {"S": "1"}, "transaction_id": {"N": "3"}, "amount": {"N": "300"}, "description": {"S": "Transaction 3"}}'
    aws dynamodb put-item --table-name UserTransactions --item '{"user_id": {"S": "1"}, "transaction_id": {"N": "4"}, "amount": {"N": "400"}, "description": {"S": "Transaction 4"}}'
    aws dynamodb put-item --table-name UserTransactions --item '{"user_id": {"S": "1"}, "transaction_id": {"N": "5"}, "amount": {"N": "500"}, "description": {"S": "Transaction 5"}}'
    aws dynamodb put-item --table-name UserTransactions --item '{"user_id": {"S": "1"}, "transaction_id": {"N": "6"}, "amount": {"N": "600"}, "description": {"S": "Transaction 6"}}'
    ```

### Step 2: Update the Chatbot Script

Modify the Python script to handle queries for the last 5 transactions of a user.

1. **Create a new Python script `transaction_chatbot.py`**:

    ```python
    import boto3
    from boto3.dynamodb.conditions import Key

    # Initialize DynamoDB client
    dynamodb = boto3.resource('dynamodb')
    table = dynamodb.Table('UserTransactions')

    def query_last_transactions(user_id, limit=5):
        try:
            response = table.query(
                KeyConditionExpression=Key('user_id').eq(user_id),
                ScanIndexForward=False,  # Get the latest transactions
                Limit=limit
            )
            items = response.get('Items')
            if items:
                return items
            else:
                return "No transactions found for user."
        except Exception as e:
            return str(e)

    def format_transactions(transactions):
        if isinstance(transactions, str):
            return transactions
        formatted = "\n".join([f"Transaction ID: {t['transaction_id']} | Amount: {t['amount']} | Description: {t['description']}" for t in transactions])
        return formatted

    def get_response(user_query):
        user_id = user_query.strip()
        transactions = query_last_transactions(user_id)
        return format_transactions(transactions)

    def main():
        print("Welcome to the User Transactions Chatbot!")
        while True:
            user_query = input("You (enter user_id): ")
            if user_query.lower() in ["exit", "quit"]:
                break
            response = get_response(user_query)
            print("Chatbot:", response)

    if __name__ == "__main__":
        main()
    ```

### Running the Chatbot

1. **Run the script**:

    ```bash
    python transaction_chatbot.py
    ```

### Sample Interaction

**Input:**

```
You (enter user_id): 1
```

**Expected Output:**

```
Chatbot: 
Transaction ID: 6 | Amount: 600 | Description: Transaction 6
Transaction ID: 5 | Amount: 500 | Description: Transaction 5
Transaction ID: 4 | Amount: 400 | Description: Transaction 4
Transaction ID: 3 | Amount: 300 | Description: Transaction 3
Transaction ID: 2 | Amount: 200 | Description: Transaction 2
```

### Explanation

1. **query_last_transactions**: This function queries the `UserTransactions` table for the latest transactions of a user, limited to a specified number (default is 5).

2. **format_transactions**: This function formats the retrieved transactions into a readable string format.

3. **get_response**: This function handles the user query by retrieving and formatting the last transactions.

4. **main**: This function runs the chatbot, accepting user IDs as input and displaying the last transactions for the given user ID.

With this setup, you can now query the chatbot for the last 5 transactions of any user by entering their user ID.
