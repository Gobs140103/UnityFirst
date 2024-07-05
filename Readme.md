For extracting numbers and tables with high accuracy, especially from structured documents like invoices, receipts, or forms, you might want to consider using specialized OCR solutions. Here are some advanced options:

### 1. **AWS Textract**
Amazon Textract is a machine learning service that automatically extracts text, handwriting, and data from scanned documents. It can also recognize forms and tables.

**Installation:**
```bash
pip install boto3
```

**Usage:**
```python
import boto3

def analyze_document(document_path):
    with open(document_path, 'rb') as document:
        imageBytes = bytearray(document.read())

    client = boto3.client('textract')

    response = client.analyze_document(
        Document={'Bytes': imageBytes},
        FeatureTypes=['TABLES', 'FORMS']
    )

    for block in response['Blocks']:
        if block['BlockType'] == 'TABLE':
            print("Table detected:")
            for relationship in block.get('Relationships', []):
                if relationship['Type'] == 'CHILD':
                    for child_id in relationship['Ids']:
                        child = next(item for item in response['Blocks'] if item['Id'] == child_id)
                        print(f"Detected text: {child['Text']}")

document_path = 'path_to_your_document.jpg'
analyze_document(document_path)
```

### 2. **Google Cloud Vision**
Google Cloud Vision API is a powerful tool for detecting text in images, including handwriting, printed text, and structured data like tables.

**Installation:**
```bash
pip install google-cloud-vision
```

**Usage:**
```python
from google.cloud import vision
import io

def detect_document_text(image_path):
    client = vision.ImageAnnotatorClient()

    with io.open(image_path, 'rb') as image_file:
        content = image_file.read()

    image = vision.Image(content=content)
    response = client.document_text_detection(image=image)
    document = response.full_text_annotation

    for page in document.pages:
        for block in page.blocks:
            for paragraph in block.paragraphs:
                for word in paragraph.words:
                    word_text = ''.join([symbol.text for symbol in word.symbols])
                    print(f'Word text: {word_text}')

image_path = 'path_to_your_image.jpg'
detect_document_text(image_path)
```

### 3. **Microsoft Azure Cognitive Services**
Azure's Computer Vision API provides advanced OCR capabilities, including structured data extraction from forms and tables.

**Installation:**
```bash
pip install azure-cognitiveservices-vision-computervision
```

**Usage:**
```python
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from azure.cognitiveservices.vision.computervision.models import OperationStatusCodes
from msrest.authentication import CognitiveServicesCredentials
import time

def azure_ocr(image_path):
    subscription_key = "your_subscription_key"
    endpoint = "your_endpoint"
    
    client = ComputerVisionClient(endpoint, CognitiveServicesCredentials(subscription_key))

    with open(image_path, "rb") as image_stream:
        read_response = client.read_in_stream(image_stream, raw=True)
    
    read_operation_location = read_response.headers["Operation-Location"]
    operation_id = read_operation_location.split("/")[-1]
    
    while True:
        read_result = client.get_read_result(operation_id)
        if read_result.status not in [OperationStatusCodes.running, OperationStatusCodes.not_started]:
            break
        time.sleep(1)
    
    if read_result.status == OperationStatusCodes.succeeded:
        for text_result in read_result.analyze_result.read_results:
            for line in text_result.lines:
                print(f"Detected text: {line.text}")

image_path = 'path_to_your_image.jpg'
azure_ocr(image_path)
```

### Summary

1. **AWS Textract**: Specialized in extracting text, forms, and tables from scanned documents.
2. **Google Cloud Vision**: Advanced text detection, including handwriting and structured data.
3. **Microsoft Azure Cognitive Services**: Provides robust OCR capabilities with structured data extraction.

These cloud-based solutions typically offer the highest accuracy and can handle complex documents with numbers and tables effectively. They require setting up API keys and endpoints but provide powerful capabilities beyond local libraries.
