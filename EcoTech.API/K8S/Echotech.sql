Create Database EchoTech;
GO
Use EchoTech;
GO

CREATE TABLE EchoTech_Roles(
	RoleId int IDENTITY(1,1) NOT NULL,
	RoleName nvarchar(15) NOT NULL,
 CONSTRAINT PK_EchoTech_Roles_RoleId PRIMARY KEY (RoleId),
 Constraint Unique_EchoTech_Roles_RoleName Unique (RoleName)
)

Insert into EchoTech_Roles (RoleName) Values ('SuperAdmin')
Insert into EchoTech_Roles (RoleName) Values ('Admin')
Insert into EchoTech_Roles (RoleName) Values ('Manager')

Go

Create Table EchoTech_Subscription_Types(
SubId int Identity(1,1),
SubName char(32) Not Null,
PeriodInMonths int Not Null,
Constraint PK_EchoTech_Subscription_Types_SubId Primary Key (SubId),
Constraint Unique_EchoTech_Subscription_Types_SubName Unique (SubName))


Insert into EchoTech_Subscription_Types(SubName,PeriodInMonths) Values ('Free Trail',1),('Half Year',6),('Annual',12)	
Go


Create Table EchoTech_Logs(
Id bigint Not Null Identity(1,1),
UniqueId nvarchar(31) Null,
Information nvarchar(4000) Null,
Constraint PK_Echotech_Logs_Id Primary Key (Id))

Go

CREATE TABLE EchoTech_OTPs(
	OtpId int Identity(1,1),
	Contact char(159) NOT NULL,
	OTP int NOT NULL,
	CreatedAt datetime NOT NULL,
	Constraint PK_EchoTech_OTPs_OtpId Primary Key (OtpId),
	Constraint Unique_EchoTech_OTPs_Contact Unique (Contact))

Go

CREATE  TABLE EchoTech_Clients(
	ClientId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(200) NOT NULL,
	Address nvarchar(400) NULL,
	GSTIN_or_PAN nvarchar(15) NULL,
	MobileNumber bigint NOT NULL ,
	IsActive bit NOT Null,
	FreeTrialAvailed bit not null default 0,
	 CONSTRAINT PK_EchoTech_Clients_ClientId Primary Key  (ClientId))

Go

Create  Table EchoTech_Subscriptions(
ClientSubId bigint Identity(1,1),
ClientId int Not Null,
SubId int Not Null,
UsedUser TinyInt Not Null default 0,
StartDate Date Not Null default GetDate(),
EndDate Date Not Null,
UserIds char(95),
Constraint PK_EchoTech_Subscriptions_ClientSubId Primary Key (ClientSubId),
Constraint FK_EchoTech_Subscriptions_ClientId Foreign Key (ClientId) References EchoTech_Clients(ClientId) on Delete Cascade,
Constraint FK_EchoTech_Subscriptions_SubId Foreign Key (SubId) References EchoTech_Subscription_Types(SubId)
)
	 
Go	

 CREATE  TABLE EchoTech_Users(
	UserId bigint IDENTITY(1,1) NOT NULL,
	ClientId int NOT NULL,
	Name nvarchar(104)  NULL,
	MobileNumber bigint NOT NULL,
	SessionId uniqueidentifier NULL,
	UserEmail varchar(159) NULL,
	ClientSubId bigint Null,
	IsActive bit not null,
	RoleId int null,
 CONSTRAINT PK_EchoTech_Users_UserId PRIMARY KEY (UserId),
 Constraint Unique_EchoTech_Users_MobileNumber Unique (MobileNumber),
 Constraint FK_EchoTech_Users_ClientId Foreign Key (ClientId) references EchoTech_Clients (ClientId) on Delete Cascade,
Constraint FK_EchoTech_Users_ClientSubId Foreign Key (ClientSubId) References EchoTech_Subscriptions (ClientSubId),
 Constraint FK_EchoTech_Users_RoleId Foreign Key
(RoleId) References EchoTech_Roles (RoleId)) 

 

Go
 



Create Table EchoTech_Subscriptions_Consumed(
SubConsumptionId bigint Identity(1,1),
ClientSubId bigint Not Null,
UserId bigint Not Null,
Sub_StartDate Date Not Null,
Sub_EndDate Date Null,
Constraint PK_EchoTech_Subscriptions_Consumed_SubConsumptionId Primary Key (SubConsumptionId),
Constraint FK_EchoTech_Subscriptions_Consumed_ClientSubId Foreign Key (ClientSubId) References EchoTech_Subscriptions (ClientSubId) on Delete Cascade,
Constraint Fk_EchoTech_Subscriptions_Consumed_UserId Foreign Key (UserId) References EchoTech_Users(UserId))



Go



Create  OR ALTER  Procedure USP_Log_Exceptions(
@UniqueId nvarchar(31) =Null,
@Information nvarchar(4000)=Null)

AS
Begin
	Insert Into EchoTech_Logs(UniqueId,Information) Values (@UniqueId,@Information)
End

GO


Create OR ALTER Procedure [dbo].[USP_Get_Roles](
@MobileNumber bigint )

As
Begin
	
	If @MobileNumber is Null
	Begin
		Select 0 As Result, 'Please provide valid Mobile Number' As Message
		Return;
	End
	
	Declare @UserId bigint= (Select UserId From EchoTech_Users where MobileNumber=@MobileNumber);
	If @UserId is Null OR @UserId <0
	Begin
		Select 0 As Result, 'User does not exists with given mobile number' As Message
		Return;
	End

	

	Select 1 As Result,
	(Select RoleName From EchoTech_Roles where RoleId=(Select RoleId From EchoTech_Users where UserId=@UserId))
	As Message
	

End


Go


Create OR Alter   PROCEDURE Usp_Manage_OTP(
    @Contact char(159) = NULL,  -- Optional, can be NULL
    @Otp int = NULL,               -- Optional, can be NULL
    @Operation char(10),            -- Required operation type: "Sent", "Verify", or "Delete"
	@Purpose char(15) = Null)
AS
BEGIN
    -- Validate operation type
    IF @Operation NOT IN ('Sent', 'Verify', 'Delete')
		BEGIN
			SELECT 0 AS Result, 'Invalid operation specified' AS Message;
			RETURN;
		END

    
    IF (@Operation = 'Sent' OR @Operation = 'Verify')
       AND (@Contact IS NULL OR @OTP IS NULL)
		BEGIN
			SELECT 0 AS Result, 'Contact and OTP are required' AS Message;
			RETURN;
		END

    IF @Operation = 'Sent'
    BEGIN       
        IF EXISTS (SELECT 1 FROM EchoTech_OTPs WHERE Contact = @Contact)
			BEGIN
				UPDATE EchoTech_OTPs
				SET OTP = @Otp, CreatedAt = GETDATE()
				WHERE Contact = @Contact;			
			END
        ELSE
			BEGIN
				INSERT INTO EchoTech_OTPs (Contact, OTP, CreatedAt)
				VALUES (@Contact, @Otp, GETDATE());			
			END

        SELECT 1 AS Result, 'OTP Saved' AS Message;
		Return;
    END

    ELSE IF @Operation = 'Verify'
		BEGIN
			IF EXISTS (SELECT 1 FROM EchoTech_OTPs WHERE Contact = @Contact AND OTP = @Otp)
				BEGIN
					If @Purpose='Login' 
						Begin
							Declare @UserId bigint;  Declare @SubId bigint;
							Select @UserId=UserId,@SubId=ClientSubId  From EchoTech_Users where MobileNumber=@Contact;
							If @UserId is Null
								Begin
								SELECT 0 AS Result, 'OTP verified but User does not exist' AS Message;
								Return;
								End
							If @SubId is Null
								Begin
								SELECT 0 AS Result, 'OTP verified but not subscribed' AS Message;
								Return;
								End

						
							If (GETDATE() < (Select Top 1 EndDate From EchoTech_Subscriptions where ClientSubId=@SubId))
								Begin 
									SELECT 1 AS Result, 'OTP verified' AS Message;
									Exec USP_Get_Roles @Contact;
									Return;
								End
							Else
								Begin
									SELECT 0 AS Result, 'OTP verified but subscription expired' AS Message;
									Return;
								End
						End
					SELECT 1 AS Result, 'OTP verification successful' AS Message;
					Return;
				END
			ELSE
				BEGIN
					SELECT 0 AS Result, 'OTP verification failed' AS Message;
				END
		END
	
    ELSE IF @Operation = 'Delete'
		BEGIN
			-- Delete records older than 10 minutes
			DELETE FROM EchoTech_OTPs
			WHERE DATEDIFF(MINUTE, CreatedAt, GETDATE()) > 10;
			SELECT 1 AS Result, 'Old OTPs deleted successfully' AS Message;
		END
END;




Go




Create OR Alter  Procedure USP_Client_SignUp(
@ClientName nvarchar(200) ,
@UserMobileNumber bigint,
@Name nvarchar(100)=NULL,
@UserEmail nvarchar(100)=null,
@Address nvarchar(400) =null,
@GSTIN nvarchar(15)= null)
As
Begin
	If  @ClientName is null or @UserMobileNumber is null
	Begin 
		Select 0 As Result, 'Parameters can not be null' As Message
		Return;
	End 
	 IF EXISTS (SELECT 1 FROM EchoTech_Users WHERE MobileNumber = @UserMobileNumber and IsActive=1)
	Begin 
		Select 0 As Result, 'User with given Mobile Number already exists' As Message
		Return;
	End 

	Begin Try
		Begin Transaction;
				Insert into EchoTech_Clients (Name,Address,GSTIN_or_PAN,IsActive) 
				values (@ClientName,@Address,@GSTIN,1);
				Declare @NewClientId int=SCOPE_IDENTITY();
				Insert into EchoTech_Users (ClientId,Name,MobileNumber,IsActive,UserEmail) values 
						(@NewClientId,@Name,@UserMobileNumber,1,@UserEmail);
				
		Commit Transaction;
		Select 1 As Result, 'Client Added Successfuly' As Message;
	End Try
	Begin Catch
		Rollback Transaction;
		Select 0 As Result, Error_Message() As Message
	End Catch
END

Go

Create Or ALTER Procedure [dbo].[USP_Check_User_Availability](
@UserUniqueId nvarchar(159),
@Operation nvarchar(31))
AS
Begin
		If @UserUniqueId is Null
		Begin
		Select 0 As Result, 'Please provide valid Mobile Number' As Message;
		Return;
		End
		 
		Begin Try
		If @Operation = 'UserMobileRegistration'
		Begin

			If Exists(Select * from EchoTech_Users where MobileNumber=@UserUniqueId)
			Begin
			Select 0 As Result, 'Mobile number already registered' As Message;
			Return;
			End

			Select 1 As Result, 'Mobile number is available for registration' As Message;
			Return;
		End
		
		If @Operation = 'UserEmailRegistration'
		Begin

			If Exists(Select * from EchoTech_Users where UserEmail=@UserUniqueId)
			Begin
			Select 0 As Result, 'Email already registered' As Message;
			Return;
			End

			Select 1 As Result, 'Email is available for registration' As Message;
			Return;
		End


		End Try
		Begin Catch
		Select 0 As Result, ERROR_MESSAGE() As Message;		

		End Catch
		Select 0 As Result, 'Please provide valid registration role' As Message;
End



GO

Create OR Alter Procedure USP_Start_FreeTrial(
@ClientId int)

AS
Begin
	Declare @FreeTrialAvailed Bit;
	Set @FreeTrialAvailed=(Select  FreeTrialAvailed FROM EchoTech_Clients WHERE ClientId =@ClientId and IsActive=1);
	Begin Try
		If @FreeTrialAvailed is Null
			Begin 
				Select 0 AS Result, 'Active client not found with given clientid' As Message
				Return;
			End
		Else If @FreeTrialAvailed= 1
			Begin
				Select 0 AS Result, 'Free trial already utilized' As Message
				Return;
			End
		Else If @FreeTrialAvailed =0
			Begin
				Begin Transaction;
				Update EchoTech_Clients Set FreeTrialAvailed=1 where ClientId=@ClientId;
				Declare @UserId int;
				Set @UserId = (Select UserId from EchoTech_Users where ClientId=@ClientId and RoleId =2);

				Declare @CurrentDate Date=GetDate();
				Insert Into EchoTech_Subscriptions (ClientId,SubId,UsedUser,StartDate,EndDate,UserIds)
					values (@ClientId,1,1,@CurrentDate,
					DATEADD(MONTH,(Select PeriodInMonths from EchoTech_Subscription_Types where SubName='Free Trial'),GETDATE()),
					@UserId);

				Declare @NewSubId BigInt=SCOPE_IDENTITY();				
				Update EchoTech_Users set ClientSubId=@NewSubId where UserId=@UserId;
				
				Insert into EchoTech_Subscriptions_Consumed (ClientSubId,UserId,Sub_StartDate)
					values (@NewSubId,@UserId,@CurrentDate);
				Commit Transaction;
				Select 1 AS Result, 'Free trial started' As Message
				Return;
			End
	End Try
	Begin Catch
		Rollback Transaction;
		Select 0 As Result, Error_Message() As Message;
		Return;
	End Catch
End