print("Загружаем мод...")

Events:On("GameStart", function()
    print("Игра началась!")

    UI:ShowButton("Призвать дерево", function()
        local position = { x = 10, y = 0, z = 5 }
        local id = World:Spawn("Tree", position)
        print("Посажено дерево с ID: " .. tostring(id))
    end)
end)