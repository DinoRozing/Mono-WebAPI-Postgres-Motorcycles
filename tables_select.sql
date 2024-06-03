SELECT 
    u."FirstName" AS "User_FirstName",
    u."LastName" AS "User_LastName",
    u."Email" AS "User_Email",
    m."Make" AS "Motorcycle_Make",
    m."Model" AS "Motorcycle_Model",
    m."Year" AS "Motorcycle_Year"
FROM 
    "User" u
JOIN 
    "Motorcycle" m ON u."Id" = m."CreatedByUserId";