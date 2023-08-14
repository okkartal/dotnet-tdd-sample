namespace Posts.Tests.Fixtures;

public static class PostsFixture
{
    public static List<Post> GetTestPosts()
    {
        return new()
        {
            new Post
            {
                Id = 1,
                UserId = 2,
                Title = "Some title 1",
                Body = "Some body 1"
            },
            new Post
            {
                Id = 3,
                UserId = 4,
                Title = "Some title 2",
                Body = "Some body 2"
            },
            new Post
            {
                Id = 5,
                UserId = 6,
                Title = "Some title 3",
                Body = "Some body 3"
            }
        };
    }
}