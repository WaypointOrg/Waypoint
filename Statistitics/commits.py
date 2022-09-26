"""
Génère un graphique du nombre de commits par personnes en fonction du temps
"""

from datetime import datetime
import os
import subprocess
from matplotlib import pyplot as plt
import numpy as np

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

def author_of_commit(ref):
    return subprocess.check_output(f"git show -s --format=%an {ref}", shell=True).decode().rstrip()
    
def get_contributors():
    switch_to_commit("master")
    result = subprocess.check_output("git shortlog -ns", shell=True).decode().rstrip()
    lines = result.split("\n")
    contributors = []
    for line in lines:
        parts = line.split("\t")
        contributors.append(parts[1])
    return contributors


CLONE_URL = "https://github.com/DanielRoulin/Waypoint.git"
REPO_PATH =  os.path.join(os.path.dirname(os.path.realpath(__file__)), "Waypoint")

if __name__ == "__main__":
    update_repo()
    contributors = get_contributors()
    contributors_dates_of_commit = {contributor: [] for contributor in contributors}
    for commit in list_commits():
        switch_to_commit(commit)
        contributors_dates_of_commit[author_of_commit(commit)].append(date_of_commit(commit))

    axes = plt.subplot(1, 1, 1)
    axes.set_title("Nombre de commits par personnes en fonction du temps")
    axes.set_xlabel("Date")
    axes.set_ylabel("Commits")

    for contributor, dates in contributors_dates_of_commit.items():
        axes.plot(dates, np.linspace(len(dates), 1,len(dates)), label=contributor)
    axes.legend()
    plt.show()