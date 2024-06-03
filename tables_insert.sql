INSERT INTO "User" ("FirstName", "LastName", "Email")
VALUES
    ('Dino', 'Rozing', 'dino@gmail.com'),
    ('Dora', 'Pečurlić', 'dora@gmail.com'),
    ('Nikola', 'Mavretić', 'nikola@gmail.com');

INSERT INTO "Motorcycle" ("Make", "Model", "Year", "IsDeleted", "CreatedByUserId", "UpdatedByUserId")
VALUES
    ('Honda', 'Cbr600rr', 2024, FALSE, 1, 1),
    ('Honda', 'Cb500fa', 2017, FALSE, 2, 2),
    ('Yamaha', 'R7', 2022, FALSE, 3, 3);