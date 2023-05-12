CREATE TABLE ������������� (
    id INT PRIMARY KEY,
    ��� VARCHAR(50) NOT NULL,
    �������_������ BIT NOT NULL,
    ����������_����� INT NOT NULL
);

INSERT INTO ������������� (id, ���, �������_������, ����������_�����)
VALUES (1, '������ ���� ��������', 1, 29),
       (2, '������ ���� ��������', 1, 0),
       (3, '�������� ���� ��������', 0, 50),
       (4, '������ ������ ����������', 0, 40),
       (5, '��������� ����� �������������', 0, 5);

CREATE TABLE �������� (
    id INT PRIMARY KEY,
    �������� VARCHAR(50) NOT NULL
);

INSERT INTO �������� (id, ��������)
VALUES (1, '����������'),
       (2, '������'),
       (3, '�����������'),
       (4, '�����');

CREATE TABLE �������� (
    id INT PRIMARY KEY,
    id_������������� INT NOT NULL,
    id_�������� INT NOT NULL,
    ����������_����� INT NOT NULL,
    FOREIGN KEY (id_�������������) REFERENCES �������������(id),
    FOREIGN KEY (id_��������) REFERENCES ��������(id)
);

INSERT INTO �������� (id, id_�������������, id_��������, ����������_�����)
VALUES (1, 1, 1, 10),
       (2, 1, 2, 8),
       (3, 2, 3, 2),
       (4, 3, 1, 6),
       (5, 3, 3, 4),
       (6, 4, 2, 12),
       (7, 5, 4, 20);