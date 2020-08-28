To test locally:

Windows

1. Pull repository
2. Create a copy of the folder (We need to have two copies of Unity running at once)
3. Delete the following folders from the copy: Assets, ProjectSettings (We're going to symlink these to the original so updates to one affect the other)
4. Open a command prompt as administrator and navigate to the copy folder
5. Run the following commands:
6. mklink /D Assets ..\unity-plane-game\Assets
7. mklink /D ProjectSettings ..\unity-plane-game\ProjectSettings
8. Add and open both projects via Unity Hub and move the windows so you can see each at the same time
9. Start the game in both projects
10. In one, find the DontDestroyOnLoad object in the scene and expand to see NetworkingManager, click it
11. In the Inspector Window scroll down the NetworkingManager to find 3 buttons: Start Host, Start Server, Start Client
12. Click Start Host
13. In the other project, follow the same process from step 10. on, but click Start Client
14. You should now have two players in each game. Focusing on either window and using WASD and Spacebar, you should be able to navigate the scene and see it update both games.