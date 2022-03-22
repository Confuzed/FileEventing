#!/bin/sh
export FILEEVENTING_ConnectionStrings__Files="Server=sql; Database=FileEventing; User Id=sa; Password: SQL_dev_Secret-987!"
dotnet ef migrations add -c "FileEventing.Service.FileDataContext" InitialCreate
