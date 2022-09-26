"""
Génère un graphique du nombre de lignes de code en fonction du temps
"""

from datetime import datetime
import os
import subprocess
from matplotlib import pyplot as plt

def update_repo():
    if os.path.exists(REPO_PATH):
        os.chdir(REPO_PATH)
        os.system("git pull")
    else:
        os.system(f"git clone {CLONE_URL} {REPO_PATH}")
        os.chdir(REPO_PATH)

def list_commits():
    result = subprocess.check_output("git rev-list master", shell=True).decode()
    return result.rstrip().split("\n")

def switch_to_commit(ref):
    os.system(f"git checkout {ref}")

def date_of_commit(ref):
    raw_date = subprocess.check_output(f"git show -s --format=%cI {ref}", shell=True).decode().rstrip()
    return datetime.fromisoformat(raw_date)

def count_lines():
    code_files = []
    for root, dirs, files in os.walk("Client/Assets"):
        for f in files:
            if f.endswith(".cs"):
                code_files.append(os.path.join(root, f))
    for f in os.listdir("Server"):
        if f.endswith(".cs"):
            code_files.append(os.path.join("Server", f))

    count = 0
    for code_file in code_files:
        with open(os.path.join(REPO_PATH, code_file)) as f:
            count += sum(1 for _ in f)
    return count

CLONE_URL = "https://github.com/DanielRoulin/Waypoint.git"
REPO_PATH =  os.path.join(os.path.dirname(os.path.realpath(__file__)), "Waypoint")

if __name__ == "__main__":
    update_repo()
    dates, lines = [], []
    for commit in list_commits():
        switch_to_commit(commit)
        dates.append(date_of_commit(commit))
        lines.append(count_lines())

    axes = plt.subplot(1, 1, 1)
    axes.set_title("Nombre de lignes de code en fonction du temps")
    axes.set_xlabel("Date")
    axes.set_ylabel("Lines de code")

    axes.plot(dates, lines)
    plt.show()