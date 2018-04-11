CREATE TABLE [dbo].[WebChatFriendData](
	[ID] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NULL,
	[SelfDefineDataTag] [varchar](20) NULL,
	[SelfDefineType] [nvarchar](10) NULL,
	[GroupSign] [nvarchar](10) NULL,
	[DataBelongName] [nvarchar](50) NULL,
	[DataBelongUserName] [nvarchar](100) NULL,
	[DataBelongUserNick] [nvarchar](500) NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[NickName] [nvarchar](500) NOT NULL,
	[HeadImgUrl] [nvarchar](500) NULL,
	[ContactFlag] [varchar](10) NULL,
	[MemberCount] [int] NULL,
	[MemberList] [nvarchar](10) NULL,
	[RemarkName] [nvarchar](500) NULL,
	[HideInputBarFlag] [int] NULL,
	[Sex] [int] NULL,
	[SexDesc] [nvarchar](3) NULL,
	[Signature] [nvarchar](500) NULL,
	[VerifyFlag] [int] NULL,
	[OwnerUin] [int] NULL,
	[PYInitial] [nvarchar](500) NULL,
	[PYQuanPin] [nvarchar](500) NULL,
	[RemarkPYInitial] [nvarchar](500) NULL,
	[RemarkPYQuanPin] [nvarchar](500) NULL,
	[StarFriend] [int] NULL,
	[AppAccountFlag] [varchar](20) NULL,
	[Statues] [int] NULL,
	[AttrStatus] [nvarchar](20) NULL,
	[Province] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Alias] [nvarchar](20) NULL,
	[SnsFlag] [int] NULL,
	[UniFriend] [int] NULL,
	[DisplayName] [nvarchar](20) NULL,
	[ChatRoomId] [int] NULL,
	[KeyWord] [nvarchar](1000) NULL,
	[EncryChatRoomId] [nvarchar](20) NULL,
	[IsOwner] [int] NULL,
	[uin] [nvarchar](20) NULL,
	[AliasDesc] [varchar](50) NULL
) ON [PRIMARY]