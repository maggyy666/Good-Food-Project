# GoodFood Project

## Overview
The GoodFood Project is a community-driven recipe sharing platform built using the ASP.NET MVC (Model-View-Controller) framework. This architecture separates the application logic into three interconnected components: the Model (data and business logic), the View (user interface), and the Controller (handles user input and interacts with the model and view). The platform leverages MongoDB for data storage, making it highly scalable and efficient for handling large amounts of recipe data.
## Features
- **User Recipe Submission:** Any logged-in user can contribute their culinary creations by submitting recipes through the application.
- **Image Handling:** Users can upload images of their recipes. Images are encoded in Base64 format and stored directly in the MongoDB database, ensuring that the images are always accessible and easy to manage.
- **Dynamic Content Display:** The application dynamically presents recipe data stored in the MongoDB database. This ensures that users always see the most up-to-date content.
- **Recipe Details and Listing:** Recipes are displayed dynamically on the Recipes page, where each entry links to a detailed view showing the full recipe and image.
- **User Authentication:** The platform supports user registration and login, allowing users to securely create accounts, submit recipes, and interact with the content.
- **Recipe Management:** Users can edit or delete their recipes. The edit feature pre-fills the form with existing recipe details, making it easy for users to update their submissions.
- **Responsive Design:** The platform provides a seamless user experience across various devices and screen sizes, ensuring accessibility for all users.

## Requirements
- **Development Environment:** Visual Studio Community edition (or higher) for project development.
- **Framework:** .NET Framework for application development.
- **Database:** MongoDB for storing recipe data.

## Installation
1. Clone the repository to your local machine.
   git clone https://github.com/maggyy666/goodfoodmvc.git
2. Open the project in Visual Studio.
3. Set up MongoDB: Ensure MongoDB is installed and running on your system. Configure the database connection string in the application's configuration file.
4. Import Sample Data: Download the GoodFoodDb.Recipes.json file from the repository and import it into your MongoDB instance using the following command: mongoimport --db goodfood --collection Recipes --file GoodFoodDb.Recipes.json --jsonArray


## Usage
- Launching the Application: Upon launching the application, users can navigate to the various sections, including recipe submission and viewing.
- Submitting Recipes: Users can input their recipes, including ingredients, instructions, and images. The images are encoded in Base64 format and stored in MongoDB.
- Viewing Recipes: The main interface dynamically displays recipes from the MongoDB database. Users can click on a recipe to see detailed information.
- Editing and Deleting Recipes: Users can manage their recipes directly from their profiles, with options to edit or delete entries.
- Searching Recipes: Users can search for specific recipes or ingredients, making it easier to discover new dishes.
- User Authentication: Users can register and log in to manage their submitted recipes and personalize their experience.

## Database Setup Guide
To set up the MongoDB database:
1. Install MongoDB: Follow the installation instructions provided by MongoDB for your operating system.
2. Create a Database: Open MongoDB shell and create a new database named goodfood:
3. Import Sample Data: Download the GoodFoodDb.Recipes.json file and import it into your database: mongoimport --db goodfood --collection Recipes --file GoodFoodDb.Recipes.json --jsonArray


## Contribution
Contributions to the GoodFood Project are highly encouraged. If you encounter any issues, have ideas for new features, or wish to contribute improvements, please feel free to open an issue or submit a pull request. Your contributions help enhance the GoodFood community experience for everyone.

## License

This project is licensed under the MIT License, granting users the freedom to use, modify, and distribute the software as per the terms of the license.

