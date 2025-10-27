
# Payment API with Validation Engine
A modular C++ validation engine built as a DLL that integrates with a C# ASP.NET Core Payment API via DllImport.

---

## Screens & Demo  
![Screenshot Placeholder](PaymentProcessorSimulatorAPI/screenshots/SwaggerGetRequestPage.png)  
![Screenshot Placeholder](PaymentProcessorSimulatorAPI/screenshots/SwaggerPostRequestPage.png)  
![Screenshot Placeholder](PaymentProcessorSimulatorAPI/screenshots/SwaggerPaymentSchemaPage.png)  

---

## Visiting the Website
You can visit the link [http://paymentapiwithvalidationengine-env.eba-qsdicxhu.eu-north-1.elasticbeanstalk.com/index.html](http://paymentapiwithvalidationengine-env.eba-qsdicxhu.eu-north-1.elasticbeanstalk.com/index.html) to access the latest production build of the website.

---

## Features (What You’ll See)  
- Reusable Validation Engine in modern C++17
- Pluggable Rules (e.g., amount validation, currency validation, name validation)
- Exported DLL Function (ValidatePayment) consumable by C# via P/Invoke
- Interop with ASP.NET Core Payment API (/api/nativepayment/validate)
- Error reporting with detailed messages

---

## Tech Stack  
- **Language**: C++17, C#
- **Frameworks**: ASP.NET Core 8.0 (C# Payment API), Native C++ STL (Validation Engine)
- **Interop**: P/Invoke (DllImport) integration
- **IDE**: Visual Studio 2022
- **Cloud Service**: AWS Elastic Beanstalk
- **Cloud Object Storage**: AWS S3
- **Cloud Database**: Amazon RDS for PostgreSQL
- **CI/CD Workflow**: GitHub Actions

---

## Setup (Run in 5 Minutes)  
1. Clone the repository:  
   ```bash
   git clone https://github.com/SumukhP-dev/PaymentProcessorSolution.git
   cd PaymentProcessorSimulatorAPI
   ```
2. Run 
   ```bash
   dotnet run --launch-profile https
   ```

---

## License & Contact  
**License:** MIT  

**Author:** Sumukh Paspuleti
- [LinkedIn](https://www.linkedin.com/in/sumukh-paspuleti/)  
- [Email](mailto:spaspuleti3@gatech.edu)  
