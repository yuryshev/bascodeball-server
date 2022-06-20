using Common.DbModels;
using Lobby.Models;
using Lobby.Services;
using Xunit;

namespace Lobby.Tests.Services;

public class GroupServiceTests
{
    private List<Group> _groups;
    
    public GroupServiceTests()
    {
        _groups = new List<Group>();
    }

    [Fact]
    public void IsFull_NotFullGroup_CorrectResultReturned()
    {
        // Arrange 
        var groupId = "test_id";
        // Not full group
        _groups = new List<Group>
        {
            new Group
            {
                Id = "test_id",
                Teams = new() { new() { Players = new() { } } }
            }
        };

        var service = new GroupService()
        {
            Groups = _groups
        };

        // Act
        var result = service.IsFull(groupId);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsFull_FullGroup_CorrectResultReturned()
    {
        // Arrange 
        var groupId = "test_id";
        // Full group
        _groups = new List<Group>
        {
            new Group
            {
                Id = "test_id",
                Teams = new() { 
                    new() 
                    { 
                        Players = new()
                        {
                            new User(),
                            new User(),
                        } 
                    },
                    new() 
                    { 
                        Players = new()
                        {
                            new User(),
                            new User(),
                        } 
                    },
                }
            }
        };

        var service = new GroupService()
        {
            Groups = _groups
        };

        // Act
        var result = service.IsFull(groupId);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void AddPlayer_ReconnectPlayer_GroupReturned()
    {
        // Arrange 
        var groupId = "test_id";
        var user = new User()
        {
            UserId = new Guid("11111111-1111-1111-1111-111111111111"),
            ConnectionId = "test_connection_id_correct" // correct id
        };
        var reconnectedUser = new User()
        {
            UserId = new Guid("11111111-1111-1111-1111-111111111111"),
            ConnectionId = "test_connection_id_incorrect" // incorrect id
        };
        
        // Full group
        _groups = new List<Group>
        {
            // should return this group
            new Group
            {
                Id = groupId,
                Teams = new() { new() { Players = new() { user } } }
            },
            new Group
            {
            Id = "test_id2",
            Teams = new() { new() { Players = new() { new User() } } }
            }
        };

        var service = new GroupService()
        {
            Groups = _groups
        };

        // Act
        var result = service.AddPlayer(reconnectedUser);
        
        // Assert
        Assert.Equal(groupId, result.Id);
    }
    
    [Fact]
    public void AddPlayer_NoGroupCreated_NewGroupWithUserReturned()
    {
        // Arrange 
        var user = new User()
        {
            UserId = new Guid("11111111-1111-1111-1111-111111111111"),
            ConnectionId = "test_connection_id_correct" // correct id
        };
        
        // Empty group
        _groups = new List<Group>();

        var service = new GroupService()
        {
            Groups = _groups
        };

        // Act
        var result = service.AddPlayer(user);
        
        // Assert
        Assert.NotEmpty(service.Groups);
        Assert.Equal(user, result.Teams.SelectMany(t => t.Players).First());
    }
    
    [Fact]
    public void AddPlayer_GroupAlreadyCreated_PlayerAdded()
    {
        // Arrange 
        var groupId = "test_id";
        var user = new User()
        {
            UserId = new Guid("11111111-1111-1111-1111-111111111111"),
            ConnectionId = "test_connection_id_correct" // correct id
        };
        
        // Group with space to adding new users
        _groups = new List<Group>
        {
            // should add to this group
            new Group
            {
                Id = groupId,
                Teams = new() { new() { Players = new() { new User() } } }
            },
            new Group
            {
                Id = "test_id2",
                Teams = new() { new() { Players = new() { new User(), new User() } } }
            }
        };

        var service = new GroupService()
        {
            Groups = _groups
        };

        // Act
        var result = service.AddPlayer(user);
        
        // Assert
        Assert.Equal(groupId, result.Id);
        Assert.Contains(user, result.Teams.SelectMany(t => t.Players));
    }
    
    [Fact]
    public void AddPlayer_OnlyFullGroupCreated_CreateGroupAndPlayerAdded()
    {
        // Arrange 
        var groupId = "test_id";
        var user = new User()
        {
            UserId = new Guid("11111111-1111-1111-1111-111111111111"),
            ConnectionId = "test_connection_id_correct" // correct id
        };
        
        // Group with space to adding new users
        _groups = new List<Group>
        {
            // should add to this group
            new Group
            {
                Id = groupId,
                Teams = new()
                {
                    new() { Players = new() { new User(), new User() } },
                    new() { Players = new() { new User(), new User() } }
                }
            },
            new Group
            {
                Id = groupId,
                Teams = new()
                {
                    new() { Players = new() { new User(), new User() } },
                    new() { Players = new() { new User(), new User() } }
                }
            }
        };

        var service = new GroupService()
        {
            Groups = _groups
        };

        // Act
        var result = service.AddPlayer(user);
        
        // Assert
        Assert.Equal(3, service.Groups.Count);
        Assert.NotEqual(groupId, result.Id);
        Assert.Contains(user, result.Teams.SelectMany(t => t.Players));
    }
}