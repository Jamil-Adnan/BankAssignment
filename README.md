# Beskrivning

This bank application is aimed at 2 different user groups, but maily for the bank customers. It is a web pased application that implies that the customer can use this app remotely. however there is one section in this app that could be used in the ATM machines also. 
The Admin has the opportunity to see the details of all the registered bank account information. on the home page there is a simple button that navigates the admin to a another page and displays related account info. To protec sophisticated information, the app shows only account holders name, account id, account number and current balance.
The user can navigate to a single account display information page by clicking on a button. On that page, the user is asked to insert the account ID and will display relevant onfo on that particular account. The app filters information and displays only account holders name, customer ID and current balance.
The user can deposit or withdraw Cash and this menu could be utilised in ATM machines. While inserting the account ID the system displays current balance and the user has the opportunity to deposit or withdraw in thta account. On succesful transaction the app shows the current balance and displays a message.
The user also has the possibility to transfer money to another bank or to another account in the same bank. The app requires senders account ID, bank name (optional) and receivers account ID and the amount to be sent. On succesful completion the user receives a success message on screen. The user can check the current balance afterwards by accessing the single bank account info with the account ID.
The app keep track of all the transaction and upfates the database. There is a menu which shows the last 5 ransaction on a specific account which requires the account Id to operate.

# Architecture

This app is built on database first and headless architecture principles. DotNet 8 is used and all the Nuget packages are used is also compatible with DotNet8. 
The app has 2 diferent sections, the API and the Client. The API section is responsible for all the back-end integrations. The API interconnects with the databse in local host and uses the HTTP methods to create, receive, update and delete necessary information. The API section itself is an independent section which can oprate itself and has the possibility to display the fetched information (headless architecture). In this app Swagger is used to display information and to test the functionality of the API. 
The Client section is based on Blazor and is responsible for fetching data from the API and with the help of HTML, CSS, Java Script and web browser displays in Front-end. all kinds of user interaction also take place in the client section through web browser.
While using the application both API and Client operates independently and simulstaneosly.

# Input Quality Check

The user inserts diferent types of information in the app to operate for example AccountID. The API searches for the specific Account ID in the database and makes sure that it is an element in the database. Otherwise it sends an message to the user
When inserting amount to deposit, witdraw or transfer the API rejects negative and 0 amount as input.
While withdraw and transfer the API checks if the amount is larger than the current account balance. 

# Disclaimer and potential

Because of time strain it was not possible to seperate the user and admin access which could be increase the security of the app. A lot of works is to be done in the front end as the front wnd is not attractive at all. More time and focus is needed after decorating the front end. 
The app is tested for functionality and it does what it is designed for.




