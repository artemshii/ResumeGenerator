Resume Generator App

A resume creation platform that generates ATS-friendly PDFs with flexible input methods.

Features

Resume Builder: Users can either fill out details manually or provide key points, which are expanded into a full resume using AI.

PDF Generation: Final resumes are formatted and exported using PdfSharp.

Authentication & Payments: Secured with email confirmation and integrated with Stripe for credit balance top-ups.

Technologies & Skills

EF Core, Identity Core, PdfSharp, Stripe Integration, OpenAI API (via HttpClient)

Setup

Clone the repository:

git clone https://github.com/artemshii/ResumeGenerator/


Add a valid appsettings.json file with the necessary configuration (database, Stripe keys, OpenAI API key, etc.).

Run the application:

dotnet run

P.S only one style added right now, new styles will be added soon
