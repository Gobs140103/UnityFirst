To create a strong OCR system using only pip-installable libraries, you can combine several libraries for various stages of the OCR process, such as image preprocessing, text detection, and text recognition. Here is a step-by-step guide to build a robust OCR system:

### Step 1: Install Required Libraries

First, install the necessary libraries:

```bash
pip install opencv-python-headless easyocr numpy matplotlib
```

### Step 2: Image Preprocessing

Preprocessing the image can significantly improve OCR accuracy. Common preprocessing steps include resizing, converting to grayscale, thresholding, and denoising.

Here's an example using OpenCV:

```python
import cv2
import numpy as np

def preprocess_image(image_path):
    # Read the image
    image = cv2.imread(image_path)
    
    # Convert to grayscale
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    
    # Apply Gaussian Blur to reduce noise
    blurred = cv2.GaussianBlur(gray, (5, 5), 0)
    
    # Apply thresholding
    _, thresh = cv2.threshold(blurred, 0, 255, cv2.THRESH_BINARY + cv2.THRESH_OTSU)
    
    return thresh

image_path = 'path_to_your_image.jpg'
preprocessed_image = preprocess_image(image_path)
cv2.imwrite('preprocessed_image.jpg', preprocessed_image)
```

### Step 3: Text Detection and Recognition

EasyOCR is a great library for text detection and recognition. It's based on deep learning and can handle multiple languages.

Here's an example using EasyOCR:

```python
import easyocr
import matplotlib.pyplot as plt

def perform_ocr(preprocessed_image_path):
    # Initialize EasyOCR reader
    reader = easyocr.Reader(['en'])  # Specify the languages
    
    # Perform OCR on the preprocessed image
    results = reader.readtext(preprocessed_image_path)
    
    return results

preprocessed_image_path = 'preprocessed_image.jpg'
ocr_results = perform_ocr(preprocessed_image_path)

# Display OCR results
for (bbox, text, prob) in ocr_results:
    print(f'Text: {text}, Probability: {prob}')
    
    # Draw bounding box on the image
    top_left = tuple(bbox[0])
    bottom_right = tuple(bbox[2])
    preprocessed_image = cv2.imread(preprocessed_image_path)
    cv2.rectangle(preprocessed_image, top_left, bottom_right, (0, 255, 0), 2)
    
plt.imshow(cv2.cvtColor(preprocessed_image, cv2.COLOR_BGR2RGB))
plt.axis('off')
plt.show()
```

### Step 4: Save OCR Output to a Text File

Finally, save the OCR results to a text file:

```python
def save_ocr_results_to_file(results, output_file):
    with open(output_file, 'w') as f:
        for (bbox, text, prob) in results:
            f.write(f'Text: {text}, Probability: {prob}\n')

output_file = 'ocr_results.txt'
save_ocr_results_to_file(ocr_results, output_file)
```

### Summary

This pipeline combines image preprocessing using OpenCV and text detection and recognition using EasyOCR. By following these steps, you should be able to create a strong OCR system using only pip-installable libraries. Adjust the preprocessing parameters and the OCR configurations as needed to optimize for your specific use case.
