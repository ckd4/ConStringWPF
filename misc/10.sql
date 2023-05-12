CREATE TABLE Преподаватели (
    id INT PRIMARY KEY,
    ФИО VARCHAR(50) NOT NULL,
    Штатный_статус BIT NOT NULL,
    Количество_часов INT NOT NULL
);

INSERT INTO Преподаватели (id, ФИО, Штатный_статус, Количество_часов)
VALUES (1, 'Иванов Иван Иванович', 1, 29),
       (2, 'Петров Петр Петрович', 1, 0),
       (3, 'Сидорова Анна Игоревна', 0, 50),
       (4, 'Козлов Сергей Викторович', 0, 40),
       (5, 'Михайлова Ольга Александровна', 0, 5);

CREATE TABLE Предметы (
    id INT PRIMARY KEY,
    Название VARCHAR(50) NOT NULL
);

INSERT INTO Предметы (id, Название)
VALUES (1, 'Математика'),
       (2, 'Физика'),
       (3, 'Информатика'),
       (4, 'Химия');

CREATE TABLE Нагрузка (
    id INT PRIMARY KEY,
    id_преподавателя INT NOT NULL,
    id_предмета INT NOT NULL,
    Количество_часов INT NOT NULL,
    FOREIGN KEY (id_преподавателя) REFERENCES Преподаватели(id),
    FOREIGN KEY (id_предмета) REFERENCES Предметы(id)
);

INSERT INTO Нагрузка (id, id_преподавателя, id_предмета, Количество_часов)
VALUES (1, 1, 1, 10),
       (2, 1, 2, 8),
       (3, 2, 3, 2),
       (4, 3, 1, 6),
       (5, 3, 3, 4),
       (6, 4, 2, 12),
       (7, 5, 4, 20);