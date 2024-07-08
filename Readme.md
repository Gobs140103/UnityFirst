To run the entire receipt parsing project using PaddleOCR for OCR, spaCy for NER, and Python scripts to manage preprocessing, training, and parsing, follow these steps:

### Prerequisites

1. **Environment Setup**:
   - Ensure you have Python installed (preferably Python 3.6 or higher).
   - Set up a virtual environment (optional but recommended).

2. **Install Dependencies**:
   - Install required Python packages using `pip` from the `requirements.txt` file provided in your project directory.
     ```bash
     pip install -r requirements.txt
     ```
   - Additionally, download the spaCy English model using:
     ```bash
     python -m spacy download en_core_web_sm
     ```

3. **Prepare Receipt Images**:
   - Place your receipt images in a directory (`data/receipt_images/` or as per your structure).

4. **Annotation**:
   - Use a tool like LabelImg to annotate your receipt images for entities like `DATE`, `TOTAL`, `ITEM`, `PRICE`, `TAX`, etc., and save annotations in Pascal VOC format.

### Running the Project

Here’s how to run each part of the project:

#### 1. Preprocessing Receipt Images

Run the preprocessing script to prepare receipt images for OCR:

```bash
python scripts/preprocess.py
```

This script preprocesses images by converting them to grayscale and applying thresholding to enhance text visibility.

#### 2. Performing OCR Using PaddleOCR

Run the OCR script to extract text from preprocessed receipt images:

```bash
python scripts/ocr.py
```

This script uses PaddleOCR to perform Optical Character Recognition on preprocessed images. Adjust configurations in `ocr.py` as needed, such as language (`lang='en'`), OCR confidence thresholds, etc.

#### 3. Converting Annotations to spaCy Format

Convert annotations from Pascal VOC format to spaCy-compatible JSON format:

```bash
python utils.py
```

This script converts annotations (`annotations/annotations.xml`) to JSON format (`annotations/train_data.json`) used for training the spaCy NER model.

#### 4. Training the NER Model

Train the spaCy NER model using the converted annotations:

```bash
python scripts/train_ner.py
```

This script trains a custom NER model using spaCy on the annotated data (`train_data.json`). Adjust training iterations (`itn`), dropout (`drop`), and other parameters as needed in `train_ner.py`.

#### 5. Parsing Receipts

Parse receipt information using the trained OCR and NER components:

```bash
python scripts/parse_receipt.py
```

This script performs OCR on receipt images (`ocr.py`) and then applies the trained spaCy NER model (`ner_model_path`) to extract structured information (`parse_receipt.py`). Customize `parse_receipt.py` to structure output JSON (`result_json`) according to your requirements.

### Example Customization

- **Modify Scripts**: Edit scripts (`ocr.py`, `parse_receipt.py`, etc.) to adjust configurations, paths, and output formats based on your specific needs.
  
- **Extend Annotations**: Add more annotations (`annotations.xml`) and update training data (`train_data.json`) to include additional receipt fields like `TAX`, `PAYMENT_METHOD`, etc.

### Workflow Tips

- **Validation**: Validate each step with sample data to ensure accuracy and adjust parameters (`thresholds`, `training iterations`, etc.) as necessary.
  
- **Iterative Refinement**: Iterate on annotations, model training, and script adjustments based on testing results and feedback.

By following these steps, you can effectively run and manage your receipt parsing project using PaddleOCR for OCR, spaCy for NER, and Python scripts for preprocessing, training, and parsing, tailored to extract structured information from receipt images. Adjustments and refinements during testing will help optimize accuracy and performance for your specific application requirements.
