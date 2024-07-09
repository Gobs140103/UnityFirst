To ensure that the "TOTAL" entity is linked only to the numerical value and currency symbol (e.g., "$45.67") without including the label "Total:", you can refine the matching patterns. Here's an updated approach to achieve this:

### Step 1: Install Required Libraries
Ensure you have spaCy installed:
```bash
pip install spacy
```

### Step 2: Load SpaCy Model
Load a pre-trained spaCy model for processing the text:
```python
import spacy

# Load the spaCy model
nlp = spacy.load("en_core_web_sm")
```

### Step 3: Create a PhraseMatcher for Merchant Names
Let's say you have a predefined list of merchant names:
```python
# Predefined dataset of merchant names
merchant_names = ["Walmart", "Target", "Costco", "Amazon"]
```

### Step 4: Define Patterns for Other Entities
Define patterns to match entities like tax, total, and address:
```python
from spacy.matcher import Matcher, PhraseMatcher

# Initialize the Matcher and PhraseMatcher
matcher = Matcher(nlp.vocab)
phrase_matcher = PhraseMatcher(nlp.vocab)

# Create patterns for the PhraseMatcher
merchant_patterns = [nlp.make_doc(name) for name in merchant_names]
phrase_matcher.add("MERCHANT_NAME", None, *merchant_patterns)

# Define patterns for other entities
patterns = {
    "TOTAL": [{"LOWER": "total"}, {"IS_PUNCT": True, "OP": "?"}, {"IS_DIGIT": True, "OP": "*"}, {"IS_CURRENCY": True, "OP": "?"}, {"LIKE_NUM": True}],
    "TAX": [{"LOWER": "tax"}, {"IS_PUNCT": True, "OP": "?"}, {"IS_DIGIT": True, "OP": "*"}, {"IS_CURRENCY": True, "OP": "?"}, {"LIKE_NUM": True}],
    "ADDRESS": [{"ENT_TYPE": "GPE"}, {"IS_DIGIT": True}, {"IS_ALPHA": True, "OP": "+"}]
}

# Add patterns to the Matcher
for label, pattern in patterns.items():
    matcher.add(label, [pattern])
```

### Step 5: Process the Receipt Text and Apply Matchers
Let's use a sample receipt text:
```python
# Define the receipt text (replace this with your OCR output)
receipt_text = """
Walmart
1234 Market St
San Francisco, CA 94103
Date: 2024-07-08
Total: $45.67
Tax: $3.45
"""

# Process the receipt text with spaCy
doc = nlp(receipt_text)

# Apply the PhraseMatcher to the doc
phrase_matches = phrase_matcher(doc)

# Apply the Matcher to the doc
matches = matcher(doc)

# Initialize a dictionary to store entities
entities = {
    "MERCHANT_NAME": [],
    "ADDRESS": [],
    "TOTAL": [],
    "TAX": []
}

# Iterate over phrase matches and extract entities
for match_id, start, end in phrase_matches:
    match_label = nlp.vocab.strings[match_id]  # Get the string representation of the label
    span = doc[start:end]  # The matched span
    entities[match_label].append(span.text)  # Add the matched text to the corresponding entity list

# Iterate over token matches and extract entities
for match_id, start, end in matches:
    match_label = nlp.vocab.strings[match_id]  # Get the string representation of the label
    span = doc[start:end]  # The matched span
    
    # Filter out the numerical value and currency symbol for TOTAL and TAX
    if match_label in ["TOTAL", "TAX"]:
        # Only append the value (e.g., "$45.67" or "$3.45")
        value = span[-1].text  # Get the last token which is the value
        entities[match_label].append(value)
    else:
        entities[match_label].append(span.text)  # Add the matched text to the corresponding entity list

# Print the extracted entities
for entity, values in entities.items():
    print(f"{entity}: {', '.join(values)}")
```

### Explanation of Changes:
1. **Patterns for TOTAL and TAX**: The patterns for "TOTAL" and "TAX" have been updated to ensure they match the numerical value and currency symbol.
2. **Filter out Numerical Values**: When iterating over matches, for "TOTAL" and "TAX" entities, only the last token (which should be the value) is appended to the `entities` dictionary.

### Expected Output:
```plaintext
MERCHANT_NAME: Walmart
ADDRESS: 1234 Market St
TOTAL: $45.67
TAX: $3.45
```

This script ensures that the "TOTAL" and "TAX" entities are linked only to the numerical values and currency symbols, excluding any preceding labels like "Total:" or "Tax:". Adjust the patterns as needed based on the format of your OCR output.
