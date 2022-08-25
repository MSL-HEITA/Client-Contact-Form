CREATE TABLE Client_Contact
(
	ClientID_ContactID nvarchar(20) PRIMARY KEY,
	client_id NVARCHAR(9) FOREIGN KEY REFERENCES client (client_id),
	contact_id INT  FOREIGN KEY REFERENCES contacts (id)
);
GO