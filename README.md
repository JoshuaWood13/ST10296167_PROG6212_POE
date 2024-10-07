# Contract Monthly Claim System Prototype

## Documentation

### Design Choices
When designing the prototype for part 1 of the POE, I knew it was important to make the correct design choices now, in order to set a proper foundation for developing an efficient, scalable, and user-friendly system. I therefore made a few key design choices, such as choosing the correct architecture for my use case. 

For the framework of my system, I decided to use **.NET Core** due to its extensive support for building scalable web applications and its cross-platform capabilities which will allow the system to be run on many different devices and operating systems. The built-in security features .NET Core offers will also help ensure that the system stays secure (Microsoft, 2024).

In combination with .NET Core, I decided to use the **Model-View-Controller (MVC)** design pattern for the structure of my application. MVC allows for the simple and effective separation of program logic, allowing me to develop the front-facing GUI independently from the rest of the system and then develop the functionality and program logic of the system at a later stage (GeeksforGeeks, 2022).

### Database Structure
For this prototype, I decided to go with a **relational database structure** that will represent and structure the data across multiple tables, using primary and foreign keys to maintain relationships. After examining the POE background and system requirements, I identified 6 key entities: **Lecturers**, **Claims**, **Supporting Documents**, **Claim Approval**, **Academic Manager (AM)**, and **Programme Coordinator (PC)**. These 6 entities will contain all the necessary data required by the application and their relationships have been structured to allow for all the required functionality:

- **Lecturers** can submit many claims, creating a one-to-many relationship between the Lecturers and Claims table.
- Each claim can contain multiple supporting documents, creating a zero-to-many relationship between **Claims** and **Supporting Documents**.
- Lastly, **AMs** and **PCs** can verify and approve claims, creating two zero-to-many relationships between the **Claim Approval** table.

These relationships have been structured using the appropriate primary and foreign keys through the use of entity IDs, to allow for data integration and the database has been normalized to third normal form (3NF) in order to avoid redundancy and maintain data integrity. Therefore, this relational database structure ensures that the system can efficiently handle data and scale as needed in the future.

### GUI Layout
When designing the layout for the GUI of the prototype, it was important to ensure that the GUI was user-friendly and intuitive, so that Lecturers, AMs, and PCs of any age or digital literacy can navigate and use the system effectively. In order to accomplish this, I’ve ensured that the GUI is consistent and cohesive across all visual and navigation elements, ensuring that users can quickly become familiar with the system.

The GUI will always have a visible navigation bar, indicating all functionality available to a user depending on the account type. With each navigation bar item directing the user to a specific view. Each of these views is clearly labeled with bold headings indicating its functionality, whilst its input fields, buttons, and tables are well-labeled and organized to increase readability and reduce user errors. 

Finally, each view is associated with only one specific type of functionality, helping to increase usability and simplify the process of using the system’s various features.

### Assumptions and Constraints

#### Assumptions
When designing this prototype there were a few assumptions I made:

- I assumed that the system would require all users to have an account. That is why I implemented a login GUI for Lecturers, AMs, and PCs to each log in with their own account details.
- I assumed that Lecturers, AMs, and PCs would only be able to view and access functionality relevant to them. That is why my GUI is split to only allow AMs and PCs to verify and approve claims, and only Lecturers can submit claims, upload supporting documentation, and view their claims.
- Finally, I assumed that all users attempting to use the system will have internet access and devices capable of running web-based applications.

#### Constraints
There was also one main constraint I had to consider:

When designing this prototype, I was only allowed to work on the front-end of this system through designing the system’s GUI and database structure. This constraint means that I could not implement or test any of the underlying system functionality.

As a result of this, there may be unforeseen issues when it comes to integrating back-end functionality with the database and GUI in later stages of development. The layout and structure of the current database and GUI may need to be changed or updated to better support user interactions with the system that were not addressed in this prototype. Therefore, I recognize that the database and GUI may need to be refined once the system’s back-end functionality is developed.
