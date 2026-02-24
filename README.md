# DormitoryApp

Cross-platform **.NET MAUI** application for **searching data in XML** using different parsing approaches (**SAX / DOM / LINQ to XML**) and **transforming XML to HTML** via **XSLT**.

<img width="1920" height="965" alt="image" src="https://github.com/user-attachments/assets/c80b591e-8d43-44de-a2bc-aa53b1d4277c" />

> Educational / demo project that shows how the same XML search task can be implemented with different strategies.

---

## ✨ Features

- 📂 **Open any XML file** via file picker  
- 🔎 **Search in XML** by filters:
  - keyword (name)
  - faculty
  - department
  - course
- 🧠 **Switch parsing strategy** at runtime:
  - SAX
  - DOM
  - LINQ to XML
- 🧹 Clear filters and results
-   **Transform XML → HTML**:
  - choose an **XSL** file
  - app generates `output.html` next to the selected XML

---

## 🧱 Tech stack

- **C# / .NET MAUI**
- Multi-targeting: **Android / iOS / MacCatalyst / Windows** :contentReference[oaicite:1]{index=1}
- UI: MAUI XAML + code-behind

---

## 🏗️ Architecture (short)

The search logic is implemented via the **Strategy pattern**:

- `IXmlSearchStrategy` — common interface for searching
- `SaxSearchStrategy`, `DomSearchStrategy`, `LinqXmlSearchStrategy` — concrete strategies  
- The UI allows switching the strategy from a picker and runs the same search request with the chosen implementation. :contentReference[oaicite:2]{index=2}

Transformation is handled by a separate component (XSLT → HTML) and saves the result as `output.html`. :contentReference[oaicite:3]{index=3}

---

## 🚀 Getting started

### Prerequisites
- **Visual Studio 2022** with **.NET MAUI** workload  
  *(or .NET SDK + MAUI workload)*

### Run (Windows example)
```bash
dotnet workload install maui
dotnet build
dotnet run -f net9.0-windows10.0.19041.0
